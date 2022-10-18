# ketab-foroshi

This application is a book store managemnet, and uses C# for the backend, WPf for the GUI, and sqllite for connecting to data base. For the authentication, the passwords are hashed with the SHA 256 method, which returns an array of bytes. We then converted this array to a base64 string, and finally the result would be compared with the hashed password stored in the database (you can try admin/admin to login).
The application has following functionalities:
  - Login / sign up
  - Adding book
  - Adding customer orders
  - Create / Manage storage
  - Backup
  - Editing profile
  
use case diagram:
![image](https://user-images.githubusercontent.com/76449700/196435679-2f599f02-8a83-4633-a280-1925656622d4.png)

Class diagram:
![image](https://user-images.githubusercontent.com/76449700/196436298-1a3f2b8b-2f2e-4a99-bda6-ea634dfafc96.png)

collaboration diagram:
![image](https://user-images.githubusercontent.com/76449700/196435902-17da63d1-21a4-48a4-80f8-32962ca2d3d3.png)

State diagram:
![image](https://user-images.githubusercontent.com/76449700/196435966-48da9fc1-0277-4cf3-8fc1-9348cc65e3a9.png)

WND:
![image](https://user-images.githubusercontent.com/76449700/196436026-dd9e9803-2c81-4cc1-868c-22a44385ff6e.png)

CRUD table:
![image](https://user-images.githubusercontent.com/76449700/196436477-bd437ab5-6357-43c1-9817-4aa89e7fdb06.png)


Here are some screenshots of the application:
![image](https://user-images.githubusercontent.com/76449700/196435334-6be16e99-f96b-4799-9217-c7d988880b1a.png)
![image](https://user-images.githubusercontent.com/76449700/196435388-1b01a9cb-6a41-443c-87b1-5e7b43dc9824.png)
![image](https://user-images.githubusercontent.com/76449700/196435437-83e12f96-0f83-4087-9be3-02841bea61bf.png)
![image](https://user-images.githubusercontent.com/76449700/196435361-847a1b74-9734-4f8b-a5ce-b55a43d3c1c7.png)
![image](https://user-images.githubusercontent.com/76449700/196435478-39045c83-f52a-41a1-aeff-56cf260429d1.png)
![image](https://user-images.githubusercontent.com/76449700/196435284-f5196f3e-771d-4ecd-ac35-0b0d722d5636.png)
![image](https://user-images.githubusercontent.com/76449700/196435451-97e463e8-5fd1-4059-8f0a-120021d30577.png)
