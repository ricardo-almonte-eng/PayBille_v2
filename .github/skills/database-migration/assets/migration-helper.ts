import { MongoClient } from 'mongodb'
import mysql from 'mysql2/promise'

interface MigrationConfig {
  mysql: {
    host: string
    user: string
    password: string
    database: string
  }
  mongodb: {
    uri: string
    database: string
  }
}

export class MigrationHelper {
  private mongoClient: MongoClient
  private mysqlConnection: any

  async connect(config: MigrationConfig) {
    this.mongoClient = new MongoClient(config.mongodb.uri)
    await this.mongoClient.connect()

    this.mysqlConnection = await mysql.createConnection(config.mysql)
  }

  async migrateTable(
    tableName: string,
    collectionName: string,
    transformer: (row: any) => any
  ) {
    console.log(`Migrating ${tableName} → ${collectionName}...`)

    const [rows] = await this.mysqlConnection.query(`SELECT * FROM ${tableName}`)
    const documents = (rows as any[]).map(transformer)

    const db = this.mongoClient.db()
    const result = await db.collection(collectionName).insertMany(documents)

    console.log(`✓ Inserted ${result.insertedCount} documents`)
    return result.insertedCount
  }

  async disconnect() {
    await this.mongoClient.close()
    await this.mysqlConnection.end()
  }
}

// Example usage:
async function migrateProducts() {
  const helper = new MigrationHelper()
  await helper.connect({
    mysql: {
      host: 'localhost',
      user: 'root',
      password: 'password',
      database: 'paybille_v1'
    },
    mongodb: {
      uri: 'mongodb://localhost:27017',
      database: 'paybille_v2'
    }
  })

  await helper.migrateTable('products', 'products', (row) => ({
    name: row.product_name,
    sku: row.sku,
    price: parseFloat(row.price),
    cost: parseFloat(row.cost),
    stock: row.stock_quantity,
    category: row.category_id,
    description: row.description,
    status: row.is_active ? 'active' : 'inactive',
    createdAt: new Date(row.created_at),
    updatedAt: new Date(row.updated_at)
  }))

  await helper.disconnect()
}
