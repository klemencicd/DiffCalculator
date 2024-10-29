# DiffCalculator
1. Open the solution file with Visual Studio
2. Clean and rebuild the solution
3. Run the project with F5 or the "Start button"
4. Swagger UI page should open automatically. If it doesn't, you can manually access it by navigating to https://localhost:7281/swagger/index.html, where you can test the available endpoints.

Available endpints:
- PUT /v1/diff/1/right to add right Base64 string.
- PUT /v1/diff/1/left to add left Base64 string.
- GET /v1/diff/1 to calculate difference between right and left Base64 strings.
