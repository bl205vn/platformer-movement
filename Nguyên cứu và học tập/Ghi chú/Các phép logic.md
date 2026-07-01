## Toán tử quan hệ
Dùng để so sánh hai giá trị với nhau. Kết quả trả về là Đúng (`true`) hoặc Sai (`false`).
1. **`>`**: Lớn hơn.
2. **`<`**: Nhỏ hơn.
3. **`>=`**: Lớn hơn hoặc bằng.
4. **`<=`**: Nhỏ hơn hoặc bằng.
5. **`==`**: Bằng nhau (so sánh giá trị).
6. **`!=`**: Khác nhau (không bằng).
## Toán tử logic
Dùng để kết hợp hoặc đảo ngược các mệnh đề logic (`true`/`false`).

1. **`!`**: Phép PHỦ ĐỊNH (NOT) – Đổi `true` thành `false` và ngược lại.
   
2. **`&&`**: Phép VÀ (AND) – Trả về `true` khi **tất cả** các điều kiện đều đúng.
   
3. **`||`**: Phép HOẶC (OR) – Trả về `true` chỉ cần **một trong các** điều kiện đúng.
   
4. **`^`**: Phép HOẶC LOẠI TRỪ (XOR) – Trả về `true` khi hai vế có **giá trị trái ngược nhau** (một bên đúng và một bên sai).
   
5. **NAND** (viết tắt của **NOT AND**) là ==phép toán **PHỦ ĐỊNH của phép VÀ**==
	$\rightarrow$ Không có ký hiệu ngắn gọn riêng cho NAND. Mà phải kết hợp toán tử NOT (`!`) và AND (`&&`) thành: `!(A && B)`.
	$\rightarrow$ Trong kỹ thuật điện tử và tin học, NAND được gọi là **Cổng logic vạn năng (Universal gate)**. Lý do là vì ta có thể dùng duy nhất các cổng NAND để tạo ra tất cả các cổng logic khác như AND, OR, NOT, và XOR mà không cần thêm bất kỳ loại cổng nào khác.
	$\rightarrow$ **Công thức**: ==`NAND = !(A && B)`==
	$\rightarrow$ **Mẹo nhớ:** Kết quả của NAND luôn **ngược lại hoàn toàn** với AND.
	$\rightarrow$ **Quy tắc nhanh:** Chỉ ra kết quả Sai (`false`) khi tất cả đều Đúng (`true`).
	
	==Tự chế== các phép logic từ cổng logic **NAND**:
	- **Tạo phép NOT:** `A NAND A` (Nối hai đầu vào của NAND lại với nhau)
	- **Tạo phép AND:** `(A NAND B) NAND (A NAND B)` (Đảo ngược kết quả NAND một lần nữa)
	- **Tạo phép OR**: `(A NAND A) NAND (B NAND B)`
			1. Lấy A NAND với chính nó (để biến thành !A)
			2. Lấy B NAND với chính nó (để biến thành !B)
			3. Lấy kết quả của hai bước trên NAND với nhau → Bạn sẽ thu được kết quả giống hệt phép `A || B`.
	- **Tạo phép XOR**: `(A NAND (A NAND B)) NAND (B NAND (A NAND B))`
			1. Cho A và B qua cổng NAND thứ nhất → Thu được kết quả tạm thời \(X = !(A \&\& B)\)
			2. Lấy A NAND với X → Thu được kết quả thứ nhất
			3. Lấy B NAND với X → Thu được kết quả thứ hai
			4. Lấy hai kết quả ở bước 2 và 3 NAND với nhau → Bạn sẽ thu được kết quả của phép `A ^ B`
			   
6. Cổng **NOR** (viết tắt của **NOT OR** - Phủ định của OR) cũng ==là một **cổng vạn năng** giống hệt NAND==. Một mình cổng NOR có thể tự chế ra tất cả các cổng logic khác, bao gồm cả AND, OR và XOR.
	$\rightarrow$ Không có ký hiệu ngắn gọn riêng cho NOR
	$\rightarrow$ **Công thức lập trình:** `NOR = !(A || B)`
	$\rightarrow$ **Quy tắc nhớ:** Chỉ trả về Đúng (`true`) khi **tất cả** các điều kiện đều Sai (`false`).
	
	==Tự chế== các phép logic từ cổng logic **NOR**:
	- Tạo phép NOT (Cần 1 cổng NOR):
		  1. **Công thức:** `A NOR A`
		  2. **Mẹo nhớ:** Nối chung 2 đầu vào của cổng NOR lại với nhau.
	- Tạo phép OR (Cần 2 cổng NOR):
		  1. **Công thức:** `(A NOR B) NOR (A NOR B)`
		  2. **Mẹo nhớ:** Lấy kết quả của phép NOR đem phủ định một lần nữa (đi qua phép NOT ở trên).
	- Tạo phép AND (Cần 3 cổng NOR):
		  1. **Công thức:** `(A NOR A) NOR (B NOR B)`
		  2. **Mẹo nhớ:** Phủ định A, phủ định B → Rồi đem NOR hai kết quả đó lại.
	- Tạo phép XOR (Cần 5 cổng NOR):
		  1. **Công thức:** `[(A NOR A) NOR (B NOR B)] NOR (A NOR B)`
		  2. **Mẹo nhớ:** Kết hợp kết quả của phép AND (cần 3 cổng) và phép NOR (cần 1 cổng) vào một cổng NOR cuối cùng.

$\rightarrow$ ==Chỉ cần hiểu 4 cổng logic **AND, OR, NOT, XOR** là đã quá đủ để thiết kế và xử lý mọi bài toán logic phức tạp.==
Lý do:
- **Mọi hàm logic đều dịch được:** Theo toán học số, bất kỳ hệ thống hay điều kiện nào (ví dụ: mạch cộng tiền, mạch điều khiển đèn, khóa số) đều có thể biểu diễn qua tổ hợp của AND, OR và NOT.
- **XOR là "vũ khí bí mật":** XOR giúp bạn xử lý cực nhanh các mạch tính toán (như mạch cộng nhị phân) hoặc các mạch kiểm tra lỗi dữ liệu mà không cần xếp một đống cổng AND/OR/NOT lộn xộn.