A. Software yang digunakan
1. Visual Studios (VS) 2019 (atau versi berapapun) untuk view project & kodenya
2. SQL Server management studio (SSMS) untuk manajemen database
3. Postman atau sejenisnya untuk mengetest API

Langkah-langkah:
B. tarik seluruh project
C. Buatlah file 'appsettings.json', save file di direktori yang bersamaan dengan project ini, lalu masukkan json ke file tersebut seperti di bawah ini
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "CustomVariable": {
    "AccessTokenSeconds": 20
  },
  "ConnectionStrings": {
    "DefaultConnection": "server=.\\SQLEXPRESS;user=[user anda];password=[password anda];Database=[nama database];TrustServerCertificate=True;"
  },
  "Mailpit": {
    "Mode": "Development",
    "Host": "localhost",
    "Port": 1025,
    "From": "reminder@localhost"
  },
  
  "Jwt": {
    "Key": "THIS_IS_A_SUPER_SECRET_KEY_AT_LEAST_32_CHARS",
    "Issuer": "MyApi",
    "Audience": "MyApiUsers",
    "AccessTokenSeconds": 20
  },
}
keterangan pada JSON yang tertulis:
1. CustomVariable -> AccessTokenSeconds: untuk menentukan maksimal expire access_token dalam detik
2. ConnectionStrings -> DefaultConnection: untuk menentukan koneksi ke database tujuan
3. Mailpit: untuk setting dalam kirim email

D. Buka VS lalu open & pilih 'IngetinGwAPI.sln', atau langsung klik pada 'IngetinGwAPI.sln'
E. Buka SSMS
F. buatlah tabel pada SQL server management studio, dengen cara menulis kode 
cara membuat database & tabel dari VS ke Sql server management studio (SSMS):
1. atur terlebih dahulu tujuan database, pada connection string pada file 'appsettings.json' dengan menentukan server, nama database, user id, password
2. buka 'Package Manager Console' pada VS melalui: Tools -> NuGet Package Manager -> Package Manager Console
3. ketik 'Add-Migration [kalimat bebas]' lalu tekan enter
4. ketik 'Update-Database' lalu tekan enter
5. periksa SSMS, refresh, dan akan terlihat database dan tabel2 yang telah terbentuk

Mulai pengetesan API:
G. kembali ke project pada VS, untuk memulai debug, klik tombol 'Play' yang berwarna hijau, atau melalui Debug -> Start Debugging, atau tekan F5
H. akan otomatis muncul browser dan memperlihatkan daftar API yang bisa digunakan pada halaman swagger
I. buka Postman atau sejenisnya, lalu seluruh API dapat ditest menggunakan software tersebut.
(Perhatian: API tidak bisa dieksekusi & berfungsi jika debug dari VS tidak dijalankan)
J. jika selesai pengetesan, maka hentikan debug dengan menutup langsung browser halaman swagger, atau melalui Debug -> Stop Debugging, atau tekan Shift + F5