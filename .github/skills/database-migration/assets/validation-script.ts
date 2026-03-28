import { MongoClient } from 'mongodb'
import mysql from 'mysql2/promise'

export class MigrationValidator {
  private mongoClient: MongoClient
  private mysqlConnection: any

  async compareRowCounts(tableName: string, collectionName: string) {
    const [mysqlResult] = await this.mysqlConnection.query(
      `SELECT COUNT(*) as count FROM ${tableName}`
    )
    const mysqlCount = mysqlResult[0].count

    const mongoCount = await this.mongoClient
      .db()
      .collection(collectionName)
      .countDocuments()

    const match = mysqlCount === mongoCount
    console.log(`${tableName}: MySQL=${mysqlCount}, MongoDB=${mongoCount} ${match ? '✓' : '✗'}`)

    return match
  }

  async validateSampleRecords(
    tableName: string,
    collectionName: string,
    validator: (mysqlRow: any, mongoDoc: any) => boolean
  ) {
    const [firstRecord] = await this.mysqlConnection.query(
      `SELECT * FROM ${tableName} LIMIT 1`
    )

    if (firstRecord.length === 0) return true

    const mongoDoc = await this.mongoClient
      .db()
      .collection(collectionName)
      .findOne()

    const isValid = validator(firstRecord[0], mongoDoc)
    console.log(`${tableName} sample: ${isValid ? '✓' : '✗'}`)

    return isValid
  }

  async checkIndexes(collectionName: string, expectedIndexes: string[]) {
    const indexes = await this.mongoClient
      .db()
      .collection(collectionName)
      .listIndexes()
      .toArray()

    const indexNames = indexes.map((i) => i.name)
    let allPresent = true

    for (const index of expectedIndexes) {
      const present = indexNames.includes(index)
      if (!present) allPresent = false
      console.log(`  ${index}: ${present ? '✓' : '✗'}`)
    }

    return allPresent
  }
}
