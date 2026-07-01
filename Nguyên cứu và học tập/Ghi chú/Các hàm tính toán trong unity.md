---
Bí danh:
  - Nghiên cứu
tags:
  - Dự_án_tốt_nghiệp
Ngày: 2026-06-11
Trạng thái: 🔴Đang làm
Liên quan:
  - "[[Hiểu khi xem video về Platformer của kênh Dawnosaur]]"
  - "[[Nghiên cứu theo cách hiểu về platformer-movement-main DATA]]"
  - "[[Nghiên cứu theo cách hiểu về platformer-movement-main tính toán]]"
---


1. ==**Mathf.Pow**==: tính lũy thừa của một số.
   - **Cú pháp:** `Mathf.Pow(float f, float p)`
   - **Ý nghĩa:** Trả về kết quả của số `f` mũ `p` $(f^{p})$.
   - **Kiểu dữ liệu:** Cả tham số truyền vào và kết quả trả về đều là kiểu số thực (`float`).
     
==Ví dụ minh họa==:
```C#
float binhPhuong = Mathf.Pow(5f, 2f); // Kết quả: 25 (5 mũ 2)
float lapPhuong = Mathf.Pow(2f, 3f);  // Kết quả: 8 (2 mũ 3)
float canBacHai = Mathf.Pow(9f, 0.5f); // Kết quả: 3 (9 mũ 0.5 tương đương căn bậc 2)
```

==Lưu ý quan trọng==:
- **Sự khác biệt với C# thuần:** Trong C# tiêu chuẩn không có `Mathf`. Bạn phải dùng `Math.Pow(double x, double y)` thuộc namespace `System`, vốn sử dụng kiểu dữ liệu `double`
- **Hiệu suất (Performance):** Hàm `Mathf.Pow` xử lý khá nặng. Nếu bạn chỉ cần bình phương một số (ví dụ: $x^{2}$), hãy tự nhân trực tiếp $x \times x$ để game chạy mượt hơn.
  
2. ==**Mathf.Abs**==: dùng để lấy **giá trị tuyệt đối**
   - **Cú pháp:** `Mathf.Abs(float f)` hoặc `Mathf.Abs(int value)`
   - **Ý nghĩa:** Loại bỏ dấu âm của số truyền vào, luôn trả về một số không âm $(\vert{}f\vert{})$
   - **Kiểu dữ liệu:** Hỗ trợ cả kiểu số thực (`float`) và số nguyên (`int`). Truyền vào kiểu nào sẽ trả về kiểu đó.
     
==Ví dụ minh họa==
```C#
float a = Mathf.Abs(-7.5f); // Kết quả: 7.5f
int b = Mathf.Abs(10);      // Kết quả: 10
int c = Mathf.Abs(0);       // Kết quả: 0
```

**Ứng dụng thực tế trong làm game**

`Mathf.Abs` được sử dụng rất nhiều khi bạn chỉ quan tâm đến **độ lớn** mà không cần biết hướng:

- **Tính khoảng cách:** Tìm khoảng cách giữa hai vị trí trên một trục bằng cách lấy hiệu tọa độ của chúng cho vào hàm `Mathf.Abs` (khoảng cách thì không thể âm).
- **Kích hoạt hiệu ứng di chuyển (Animation):** Kiểm tra xem nhân vật có đang di chuyển hay không. Bạn truyền vận tốc vào `Mathf.Abs`. Nếu kết quả lớn hơn 0, chuyển từ trạng thái đứng yên (Idle) sang chạy (Run), bất kể nhân vật đang chạy sang trái hay sang phải.
- **Xử lý sát thương:** Đảm bảo lượng máu trừ đi luôn là số dương để tránh lỗi cộng thêm máu cho nhân vật.

**Sự khác biệt với C# thuần**

Hàm này hoạt động hoàn toàn giống với `System.Math.Abs` trong C# thuần. Điểm tiện lợi là `Mathf.Abs` của Unity được tối ưu sẵn cho kiểu `float`, giúp bạn không cần phải ép kiểu dữ liệu khi làm việc với tọa độ hay vận tốc trong game.
   
3. ==**Mathf.Sign**==: xác định **dấu** của một số (âm, dương hoặc bằng không) trong lập trình Unity (C#)
   - **Cú pháp:** `Mathf.Sign(float f)`
   - **Ý nghĩa:** Hàm kiểm tra giá trị của số `f` và trả về kết quả theo quy tắc sau:
     - Trả về `1` nếu `f` là số dương hoặc bằng `0`
     - Trả về `-1` nếu `f` là số âm
   - **Kiểu dữ liệu:** Tham số truyền vào là `float`, kết quả trả về cũng là `float` (`1f` hoặc `-1f`)
     
==Ví dụ minh họa==:
```C#
float ketQua1 = Mathf.Sign(10.5f); // Kết quả: 1
float ketQua2 = Mathf.Sign(-5f);   // Kết quả: -1
float ketQua3 = Mathf.Sign(0f);    // Kết quả: 1 (Lưu ý: Unity quy ước 0 là số dương)
```

**Ứng dụng thực tế trong làm game**:
Hàm này cực kỳ hữu dụng khi bạn cần biết hướng chuyển động hoặc lật hình ảnh nhân vật (Flip Sprite):

- **Lật mặt nhân vật (Sprite Flip):** Lấy dấu của vận tốc trục X để biết nhân vật đang đi sang trái hay phải, từ đó quay trục `localScale.x` theo hướng `-1` hoặc `1`.
- **Xác định hướng di chuyển:** Kiểm tra xem lực đẩy hoặc vị trí mục tiêu nằm ở phía âm hay phía dương của trục tọa độ.

**Điểm khác biệt quan trọng**:
Trong C# thuần (`System.Math.Sign`), hàm sẽ trả về `0` nếu giá trị truyền vào bằng `0`. Tuy nhiên, `Mathf.Sign` của Unity **luôn trả về 1 khi gặp số 0**.

4.  ==**Mathf.Min**==: dùng để tìm và trả về **số nhỏ nhất** trong các số được truyền vào
   - Cú pháp:
	   - `Mathf.Min(float a, float b)` (So sánh 2 số)
	   - `Mathf.Min(params float[] values)` (So sánh một danh sách nhiều số)
   - **Ý nghĩa:** Trả về giá trị có số trị nhỏ nhất trong số các tham số đầu vào.
   - **Kiểu dữ liệu:** Hỗ trợ cả kiểu số thực (`float`) và số nguyên (`int`).
     
==Ví dụ minh họa==:
```C#
float minCuaHaiSo = Mathf.Min(10.5f, 5.2f); // Kết quả: 5.2f
int minCuaDaySo = Mathf.Min(8, 3, 12, -5, 6); // Kết quả: -5
```

==Ứng dụng thực tế trong làm game==
`Mathf.Min` thường được dùng để giới hạn mức trần của một giá trị hoặc chọn mục tiêu tối ưu:

- **Giới hạn lượng máu hồi phục:** Khi nhặt bình máu, bạn không muốn máu vượt quá giới hạn tối đa.
    - Code mẫu: `currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);`
- **Giới hạn tốc độ tối đa:** Đảm bảo phương tiện hoặc nhân vật không chạy quá nhanh khi xuống dốc.
- **Tìm khoảng cách ngắn nhất:** So sánh khoảng cách giữa các kẻ địch để tìm ra mục tiêu gần nhân vật chính nhất.
  
==Điểm khác biệt với C# thuần==
Hàm này tương đương với `System.Math.Min` trong C# tiêu chuẩn. Tuy nhiên, `Mathf.Min` của Unity tiện lợi hơn ở chỗ nó cho phép bạn truyền vào **nhiều hơn 2 số** (một mảng các số) cùng một lúc, trong khi `System.Math.Min` thuần chỉ cho phép so sánh hai số với nhau trong một lần gọi hàm.

5. ==**Mathf.Clamp**==: là hàm dùng để **giới hạn một giá trị** nằm trong một khoảng cố định (giữa một giá trị tối thiểu và một giá trị tối đa) trong Unity (C#).
   Nó hoạt động giống như một "bờ rào" ngăn không cho biến số vượt quá phạm vi cho phép.

==Cú pháp và Cách hoạt động==
**Cú pháp:** `Mathf.Clamp(float value, float min, float max)`
**Ý nghĩa:** Hàm kiểm tra biến `value` và trả về kết quả theo quy tắc:
Trả về `min` nếu `value` nhỏ hơn `min`.
Trả về `max` nếu `value` lớn hơn `max`.
Trả về chính xác `value` nếu nó đã nằm sẵn trong khoảng giữa `min` và `max`.

**Kiểu dữ liệu:** Hỗ trợ cả kiểu số thực (`float`) và số nguyên (`int`).

==Ví dụ minh họa==
```C#
float ketQua1 = Mathf.Clamp(5f, 1f, 10f);  // Kết quả: 5 (Vì 5 nằm trong khoảng 1-10)
float ketQua2 = Mathf.Clamp(-3f, 1f, 10f); // Kết quả: 1 (Vì -3 nhỏ hơn mức tối thiểu)
float ketQua3 = Mathf.Clamp(15f, 1f, 10f); // Kết quả: 10 (Vì 15 lớn hơn mức tối đa)
```

==Ứng dụng thực tế trong làm game==
`Mathf.Clamp` là một trong những hàm toán học được sử dụng nhiều nhất khi lập trình Unity:

- **Giới hạn góc quay của Camera:** Ngăn camera không bị lật ngược ra phía sau khi người chơi ngước nhìn lên trời hoặc nhìn xuống đất.
    - Code mẫu: `rotationX = Mathf.Clamp(rotationX, -90f, 90f);`
- **Giới hạn vị trí di chuyển:** Không cho nhân vật đi bộ ra khỏi rìa màn hình hoặc biên giới của bản đồ.
    - Code mẫu: `pos.x = Mathf.Clamp(pos.x, minX, maxX);`
- **Quản lý thanh máu (HP):** Đảm bảo máu của nhân vật luôn nằm trong khoảng từ `0` đến `MaxHealth`.
    - Code mẫu: `hp = Mathf.Clamp(hp, 0, maxHp);`

==Biến thể đặc biệt: Mathf.Clamp01==
Unity còn cung cấp riêng hàm `Mathf.Clamp01(float value)`. Hàm này tự động giới hạn giá trị truyền vào luôn nằm trong khoảng từ **0 đến 1**. Nó cực kỳ tiện lợi khi xử lý các giá trị phần trăm, độ mờ (Alpha) của hình ảnh, hoặc tiến trình thời gian.
6. ==**Mathf.Lerp**==: dùng để **nội suy tuyến tính** giữa hai giá trị
    - **Cú pháp:** `Mathf.Lerp(float a, float b, float t)`
    - **Ý nghĩa:** Trả về một giá trị nằm giữa `a` và `b` dựa theo tỉ lệ `t`. Công thức tính là $a + (b - a) \times t$.
    - **Kiểu dữ liệu:** Tất cả tham số truyền vào và kết quả trả về đều là kiểu số thực (`float`).

==Ví dụ minh họa==:


```C#
float ketQua1 = Mathf.Lerp(0f, 10f, 0f);   // Kết quả: 0 (tại điểm đầu)
float ketQua2 = Mathf.Lerp(0f, 10f, 0.5f); // Kết quả: 5 (ở giữa)
float ketQua3 = Mathf.Lerp(0f, 10f, 1f);   // Kết quả: 10 (tại điểm cuối)
float ketQua4 = Mathf.Lerp(0f, 10f, 0.25f); // Kết quả: 2.5 (1/4 quãng đường)
```

==Lưu ý quan trọng==:

- **Tham số `t` bị giới hạn tự động:** Unity tự động kẹp giá trị `t` trong khoảng `[0, 1]`. Nếu bạn truyền vào `t = 1.5f` hay `t = -0.3f`, kết quả vẫn sẽ không vượt ra ngoài khoảng `[a, b]`.
- **Hiệu ứng "mượt dần" (Smooth Follow):** Một kỹ thuật rất phổ biến là truyền `Time.deltaTime` nhân với một hệ số tốc độ vào `t`. Vật thể sẽ di chuyển nhanh lúc đầu và chậm dần khi tiến gần đến đích.
    - Code mẫu: `transform.position = Mathf.Lerp(current, target, Time.deltaTime * speed);`

**Ứng dụng thực tế trong làm game**

`Mathf.Lerp` được dùng rất nhiều bất cứ khi nào bạn muốn tạo ra sự **chuyển đổi mượt mà** giữa hai giá trị thay vì thay đổi đột ngột:

- **Di chuyển Camera mượt mà:** Thay vì gán thẳng vị trí camera bằng vị trí nhân vật, dùng `Mathf.Lerp` để camera "đuổi theo" nhân vật một cách trơn tru.
- **Làm mờ/hiện màn hình (Fade In/Out):** Nội suy giá trị alpha của một panel đen từ `0` lên `1` để tạo hiệu ứng màn hình tối dần.
    - Code mẫu: `canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);`
- **Hồi máu/Mất máu từ từ:** Làm thanh máu trên HUD tăng hoặc giảm dần về giá trị thực thay vì nhảy số ngay lập tức.
- **Chuyển đổi âm thanh:** Tăng giảm âm lượng hoặc pitch của âm thanh một cách mượt mà.

**Sự khác biệt với C# thuần**

Trong C# tiêu chuẩn không có `Mathf.Lerp`. Bạn có thể tự viết công thức `a + (b - a) * t`, nhưng sẽ phải tự xử lý việc giới hạn `t` trong khoảng `[0, 1]`. `Mathf.Lerp` của Unity đóng gói sẵn tất cả, đồng thời được tối ưu cho kiểu `float` phổ biến trong game.