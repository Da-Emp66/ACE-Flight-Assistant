$postParams = @{command='FlyToLocation';parameters='';}
Invoke-WebRequest -URI http://localhost:8280 -Method POST -Body $postParams

# OR

Invoke-WebRequest -UseBasicParsing http://localhost:8280 -ContentType "application/json" -Method POST -Body $postParams

