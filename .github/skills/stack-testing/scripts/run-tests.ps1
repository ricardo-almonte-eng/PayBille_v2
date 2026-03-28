# PayBille v2 Test Suite Runner

Write-Host "🧪 Running PayBille v2 Tests" -ForegroundColor Cyan

# Backend tests
Write-Host "`n📦 Running .NET Tests..." -ForegroundColor Yellow
dotnet test --logger "console;verbosity=minimal"

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Backend tests failed" -ForegroundColor Red
    exit 1
}

# Frontend tests
Write-Host "`n🎨 Running Nuxt Tests..." -ForegroundColor Yellow
npm run test

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Frontend tests failed" -ForegroundColor Red
    exit 1
}

Write-Host "`n✅ All tests passed!" -ForegroundColor Green
