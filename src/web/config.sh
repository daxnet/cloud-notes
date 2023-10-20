echo Replacing environment variables
sed -i "s#__SERVICE_URL__#$SERVICE_URL#g" /usr/share/nginx/html/appsettings.Production.json
