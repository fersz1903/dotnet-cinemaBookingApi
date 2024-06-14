## Not
- Proje ayağa kaldırılmadan önce 5001, 5002, 5433, 6379 portlarının başka process tarafından kullanılmadığından emin olun.
- Ayrıca ```docker network create mynetwork``` komutu ile uygulama için bir docker ağı oluşturulmalıdır.

## Uygulamayı Başlatma
Proje klasörü içerisinde:
```docker compose up -d```
komutu ile konteynerler ayağa kaldırılabilir.

Konteynerler çalışmaya başladıktan sonra

- ```docker cp /path/to/your/dump.sql postgredb:/dump.sql``` Komutu ile veritabanı oluşturma dosyasını docker konteynerine kopyalayın,
- ```docker exec -it postgredb bash``` Komutu ile postgre konteynerine bağlanın,
- ```psql -U postgres -d cinemadb -f /dump.sql``` Komutu ile dosyayı uygulayıp veritabanını oluşturun.

Tüm bu aşamalardan sonra uygulamanın sorunsuz bir şekilde çalışması beklenmektedir.

## Kullanıcı İşlemleri
UserAuthApi'ye ```http://localhost:5002/swagger/index.html``` adresinden erişilebilir.

## Sinema İşlemleri
CinemabookingApi'ye ```http://localhost:5001/swagger/index.html``` adresinden erişilebilir.

