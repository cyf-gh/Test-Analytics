DEV=/home/ubuntu/Test-Analytics/Test-Analytics.Web/
RUN=/var/websites/test-ana/
REL='dotnet publish --configuration Release'

cd $DEV
$REL

cd /home/ubuntu/Test-Analytics/Test-Analytics.Web/bin/Release/netcoreapp2.2/

rm -rf $RUN
mkdir $RUN
mv publish/* $RUN

cd $RUN
cd ..
cp Test-Analytics.db test-ana/

netstat -anp|grep 5000

chmod 777 test-ana
chown ubuntu test-ana
chown ubuntu test-ana/*
chmod 777 test-ana/Test-Analytics.db
