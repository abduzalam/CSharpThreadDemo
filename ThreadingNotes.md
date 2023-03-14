Program

![image](https://user-images.githubusercontent.com/32676744/224982341-52466cd1-f22c-4d55-8f6e-9d9006a89d24.png)

Output

![image](https://user-images.githubusercontent.com/32676744/224982401-76c60b9a-43d7-4db7-abad-e6fc6ada4632.png)

See below program , here our aim is to print HelloWorld only one time.

Since HelloWorld method is static, both Main or Worker thread can try to access it at the same time, to avoid it we are using a lock object and set IsCompleted = true 
when the first time it is executed. later main/worker tries to execute, we do not allow it

![image](https://user-images.githubusercontent.com/32676744/224988571-874be086-46bb-4fec-a15d-a2ff594ccab7.png)


