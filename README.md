## Not
Proje ayağa kaldırılmadan önce 5001, 5002, 5433, 6379 portlarının başka process tarafından kullanılmadığından emin olun.
Ayrıca ```docker network create mynetwork``` komutu ile uygulama için bir docker ağı oluşturulmalıdır.

## Uygulamayı Başlatma
Proje klasörü içerisinde:
```docker compose up -d```
komutu ile konteynerler ayağa kaldırılabilir.

## Kullanıcı İşlemleri
UserAuthApi'ye ```http://localhost:5002/swagger/index.html``` adresinden erişilebilir.

## Sinema İşlemleri
CinemabookingApi'ye ```http://localhost:5001/swagger/index.html``` adresinden erişilebilir.

