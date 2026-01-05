0. Selamat, Anda telah berhasil pull project IngetinGwAPI
1. Buatlah file 'appsettings.json', save file di direktori yang bersamaan dengan project ini, lalu masukkan json ke file tersebut seperti di bawah ini

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
    "DefaultConnection": "server=host.docker.internal,1433;user=[user anda];password=[password anda];Database=[nama database];TrustServerCertificate=True;"
  },
  "Mailpit": {
    "Mode": "Development",
    "Host": "localhost",
    "Port": 1025,
    "From": "reminder@localhost"
  }
}

keterangan pada JSON yang tertulis:
a. CustomVariable -> AccessTokenSeconds: untuk menentukan maksimal expire access_token dalam detik
b. ConnectionStrings -> DefaultConnection: untuk menentukan koneksi ke database tujuan
c. Mailpit: untuk setting dalam kirim email

2. Buatlah database terlebih dahulu dengan buka file ScriptDB.sql, kemudian pada row paling pertama, ubah [Nama_Database_Anda] menjadi nama database sesuai nama yang anda buat
3. eksekusi seluruh script, kemudian akan terbentuk 4 tabel untuk menunjang project IngetinGwAPI
4. khusus pada tabel [Users] sudah terisi 2 user untuk nantinya dipakai percobaan.

5. buka Docker Desktop. jika belum diinstall, maka install terlebih dahulu Docker Desktop.
6. kembali menuju Git Bash, build project dengan eksekusi kode ini pada git bash: 'docker build -t ingetingwapi .' (jangan lupakan tanda titik di ujung)
7. run project dengan eksekusi kode ini pada git bash: 'docker run -p 5000:8080 ingetingwapi'
8. silahkan buka http://localhost:5000/swagger/index.html pada browser. jika terbuka, maka sudah benar

9. ke Git Bash lagi, eksekusi kode ini untuk mengaktifkan Mailpit: 'docker run -d --name mailpit -p 1025:1025 -p 8025:8025 axllent/mailpit'
10. buka http://localhost:8025 pada browser. jika terbuka, maka sudah benar

11. sekarang API sudah siap dilakukan test.
12. lakukan test API dengan menggunakan Postman atau sejenisnya