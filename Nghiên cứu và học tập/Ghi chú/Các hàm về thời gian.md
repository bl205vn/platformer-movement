1. ==Time.deltaTime== là tính thời gian bằng giây sau mỗi frame và vẫn chịu ảnh hưởng từ thời gian trong unity. Nếu muốn không bị ảnh hưởng thì dùng ==Time.unscaledDeltaTime==.
2. ==Time.time== là tổng thời gian tính bằng giây từ lúc bấm nút play game (bắt đầu từ 0), kiểu giống đồng đồ bấm giờ chạy liên tục.  Thường dùng làm cơ chế hồi chiêu và vẫn chịu ảnh hưởng từ thời gian trong unity.Nếu muốn không bị ảnh hưởng thì dùng ==Time.unscaledTime==.
   Lưu ý khi dùng ==Time.unscaledTime==: Mặc dù có thể dùng `Time.unscaledDeltaTime` để tự di chuyển vật thể bằng code (`transform.Translate`) khi pause, nhưng **hệ thống vật lý của Unity (RigidBody2D, Collider2D, các hàm OverlapBox)** sẽ **bị đóng băng hoàn toàn** khi `Time.timeScale = 0`.
3. ==Coroutine== thì hiểu rồi, để tạo nhanh xử lý thứ 2 và vẫn bị ảnh hưởng bởi thời gian trong unity 
   Mẫu: `yield return new WaitForSeconds(1f);`
   Muốn không bị ảnh hưởng khi dừng game thì dùng:
   `yield return new WaitForSecondsRealtime(1f);`