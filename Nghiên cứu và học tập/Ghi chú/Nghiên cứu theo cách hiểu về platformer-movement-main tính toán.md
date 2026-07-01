---
Bí danh:
  - Nghiên cứu
tags:
  - Dự_án_tốt_nghiệp
Ngày: 2026-06-08
Trạng thái: 🟡Tạm hoãn
Liên quan:
  - "[[Nghiên cứu theo cách hiểu về platformer-movement-main DATA]]"
  - "[[Hiểu khi xem video về Platformer của kênh Dawnosaur]]"
  - "[[Các hàm tính toán trong unity]]"
---
## Các biến
### trạng thái
1. **PlayerDataWithDash**: link đến file script PlayerDataWithDash.
2. **Rigidbody2D** biến public được đóng gói thành **{ get; private set; }** tức là chỉ có script này mới có quyền gán **Rigidbody2D** và biến *RB* ==private set== nhưng vẫn cho các script khác đọc biến và can thiệp trạng thái bên trong của biến. 
   Ngay trong script cũng nói rồi: 
   ==Đây là các trường public cho phép các script khác đọc chúng nhưng chỉ có thể ghi vào một cách private (nội bộ).==
3. **IsFacingRight**: đây khả năng là xem quay mặt sang hướng nào và là biến **bool** và được đóng gói  { get; private set; }
4. **IsJumping**: Kiểm tra xem có nhảy không và là biến **bool** và được đóng gói  { get; private set; }
5. **IsWallJumping**: Kiểm tra xem có nhảy tường không và là biến **bool** và được đóng gói  { get; private set; }
6. **IsDashing**: Kiểm tra xem có lướt không và là biến **bool** và được đóng gói  { get; private set; }
7. **IsSliding**: Kiểm tra xem có đang bám tường không và là biến **bool** và được đóng gói  { get; private set; }
8. **LastOnGroundTime**: Bộ đếm thời gian kiểm tra người chơi rời khỏi mép đất trong bao lâu và là biến **float** và được đóng gói  { get; private set; }. Gọi là ==Chạm đất còn hiệu lực== đi
   THeo tác giả nói thì **bộ đếm thời gian** như này cũng là **các trường** có thể là *private* và sử dụng một phương thức ==trả về bool==.
9. **LastOnWallTime**: Kiểm tra xem có đủ điều kiện nhảy tường không và cũng kiểm tra xem lần cuối người chơi chạm vào bất kỳ tường nào. Thường là giá trị lớn nhất giữa **LastOnWallRightTime** và **LastOnWallLeftTime**. và là biến **float** và được đóng gói  { get; private set; }
10. **LastOnWallRightTime**: Kiểm tra xem lần cuối người chơi có chạm vào bức tường bên phải không và cũng là biến kiểm tra xem khi nhảy tường thì nhân vật quay mặt sang trái. và là biến **float** và được đóng gói  { get; private set; }
11. **LastOnWallLeftTime**: Kiểm tra xem lần cuối người chơi có chạm vào bức tường bên trái không và cũng là biến kiểm tra xem khi nhảy tường thì nhân vật quay mặt sang phải. và là biến **float** và được đóng gói  { get; private set; }
12. Cả 4 biến **LastOnGroundTime**, **LastOnWallTime**, **LastOnWallRightTime**, **LastOnWallLeftTime** đều phục vụ cơ chế ==Coyote Time== (thời gian châm chước)
13. **isJumpCut**: Kiểu biến bool, khi người chơi *buôn phím nhảy* và để biến này **bật** thì hàm nào đó sẽ **tăng lực hút lên để kéo nhân vật rơi xuống đất** nhanh hơn. đây là biến **private**. Gọi là ==nhảy ngắn== đi.
14. **isJumpFalling**: chỉ bật khi biến **IsJumping** đã bật trước đố và **RB.velocity.y < 0** (tức nhân vật đã đạt đỉnh cú nhảy và bắt đầu rơi xuống). Tức biến này phục vụ cơ chế lơ lửng *trên không sau khi nhảy* (tức đã đạt đỉnh cú nhảy) **"Jump Hang"** để nhân vật dễ điều khiển trên không hơn 1 xíu. **"Là bool và private"**
    Ví dụ: Bấm nhảy               Đạt đỉnh              Rơi xuống
		   ↓                             ↓                           ↓
		IsJumping = true → velocity.y giảm → velocity.y < 0
	                               ↓
                               isJumpFalling = true ✅

15. **wallJumpStartTime**: Đếm giờ, ép nhân vật văng ra khỏi tường một thời gian ngắn trước khi cho phép bẻ lái quay lại tường để làm trò nhảy tường tiếp **"Là float và private"**
16. **lastWallJumpDir**: dùng để xác định nhân vật nên quay mặt sang trái hay phải và ngăn chặn việc nhảy liên tiếp trên cùng 1 bức tường, biến int và private
17. **dashesLeft**: Kiểu int và private , là số lần có thể lướt trước khi chạm đất.
18. **dashRefilling**: kiểu bool và private, trạng thái đánh dấu xem game có đang trong quá trình sạc lại số lần lướt không, ngăn lỗi *sạc bùm chéo* khi chạm đất. Ngăn sạc song song mà bắt **sạc lần lượt** các lượt lướt, Tức sạc xong lướt 1 rồi mới sạc lướt 2.
19. **lastDashDir**: kiểu vector2 và private, hướng lướt, kiểu biến này **lưu lại hướng** mà người chơi muốn lướt từ phím bấm như trái, phải, trên, dưới hoặc nếu không truyền vào từ phím thì mặc định theo hướng nhìn của nhân vật.
20. **isDashAttacking**: kiểu bool và private, đang trong giải đoạn lướt xung kích, do chơi theo kiểu 2 giai đoạn nên lúc lướt ban đầu là biến này là true tức là trong giải đoạn nhanh nhất (trọng lực tắt, quyền điều khiển tắt), chỉ khi sang giai đoạn phục hồi là false nhưng trạng thái IsDashing tổng vẫn true thì mới giảm tốc và trả lại chút quyền điều khiển để người chơi bẻ lái.
### Đầu vào
1. **moveInput**: kiểu vector2 và private, chỉ là lưu hướng di chuyển từ phím bấm thôi.
2. **LastPressedJumpTime**: kiểu float và public với cả đóng gói { get; private set; }, kiểu là bộ nhớ đệm khi người chơi nhấn phím nhảy mà nhân vật trong trạng thái rơi gần chạm đất rồi chạm đất trong thời gian quy định thì tự động nhấn phím nhảy. 
3. **LastPressedDashTime**: kiểu float và public với cả đóng gói { get; private set; } tương tự nhưng với lướt, khác là khi lướt gần hồi xong mà người chơi nhấn lướt trong thời gian quy định mà lướt hồi rồi = 1 thì tự động nhấn lướt.
### Kiểm tra
1. **groundCheckPoint**: kiểu transform và private, nhìn phát hiểu là cần có gameobject để cho vị trí nhận biết mặt đất rồi
2. **groundCheckSize**: kiểu vector2 và private, này là chỉnh vị trí rộng dài phần kiểm tra luôn áy. Trước làm game không nghĩ là có thể làm thế này được (kết hợp với gizmo để chỉnh trực quan hơn)
3. **frontWallCheckPoint**: kiểu transform và private, cần có gameobject để cho vị trí  nhận biết tường đằng trước nhân vật
4. **backWallCheckPoint**: kiểu transform và private, cần có gameobject để cho vị trí  nhận biết tường đằng sau nhân vật
5. **wallCheckSize**: kiểu vector2 và private tương tự **groundCheckSize** nhưng để kiểm tra tường cũng dùng được gizmo để vẽ trực quan và dùng cho cả kiểm tra tường trái phải 
=> lý do nói **frontWallCheckPoint** và **backWallCheckPoint** nói đằng trước và đằng sau là vì nhân vật có thể quay trái, quay phải nên để vậy là dễ hiểu nhất.

### Layer và tag
1. **groundLayer**: Kiểu LayerMask và private đơn giản là nhận diện lớp layer là mặt đất thôi và nó cũng làm tường luôn. Tức là dùng cho **groundCheckPoint** và **frontWallCheckPoint**với **backWallCheckPoint**.
## Logic và tính toán
1. ở hàm **Awake()** thêm component rigibody2D
2. Ở hàm **Start()** ở **SetGravityScale** lấy dữ liệu **độ lớn của lực hút**  từ **PlayerDataWithDash** với tên biến đã đặt trước đó
3. Ở hàm **Start()** ở **IsFacingRight** đặt thành *true* tức nhân vật quay mặt sang phải
### Thời gian
1. Ở hàm **Update()** cho các biến thơi gian **LastOnGroundTime**, **LastOnWallTime**, **LastOnWallRightTime**, **LastOnWallLeftTime**, **LastPressedJumpTime**, **LastPressedDashTime** trừ bằng cho **Time.deltaTime**, tức là đếm ngược theo thời gian thực (giây) và vẫn chịu ảnh hưởng bởi thời gian trong game
### Phím đầu vào
1. Về di chuyển thì dùng di chuyển cũ là **Horizontal** và **Vertical**, và phím nhảy với dash là phải gán phím, trong đó phím nhảy dùng 2 hàm: 1 là **Getkeydown** để người chơi nhảy. 2. là **getkeyup** khi người chơi buôn phím thì kích hoạt (**JumpCut**) làm nhân vật rơi nhanh hơn. với lướt thì chỉ dùng **getkeydown** thôi. Và hàm `if (_moveInput.x != 0 && !IsWallJumping) CheckDirectionToFace(_moveInput.x > 0);` là hàm để kiểm tra xem người chơi nhấn phím nào để còn cho nhân vật quay hướng đó cũng như khi bị tước quyền điều khiển trong lúc nhảy tường thì cho nhân vật quay hướng ngược lại để cho đúng logic nhảy tường.
=> Nên chuyển sang dùng New input system để làm cài đặt gán phím dễ dàng với code sạch hơn
### Cơ chế Ngắm bắn bằng Chuột (PlayerAttack) ==Bổ sung theo nhu cầu của dự án==
1. **Lấy tọa độ chuột trong Game**: Dùng hàm `_mainCamera.ScreenToWorldPoint(Input.mousePosition)` để dịch tọa độ chuột từ màn hình (pixel) sang không gian Game (tọa độ World của Unity).
2. **Kiểm tra hướng chuột**: So sánh `mouseWorld.x > transform.position.x` để xem con trỏ chuột đang nằm bên phải hay bên trái của nhân vật.
3. **Quay mặt**: Tái sử dụng hàm `CheckDirectionToFace` của `PlayerMovement` truyền giá trị ở bước 2 vào để lật ảnh nhân vật một cách tự nhiên.
4. **Khóa hướng ngắm khi nhảy tường (Anti-Bug)**: 
   - Nếu nhân vật đang bị "khóa vô lăng" do vừa nhảy bật tường ra (`IsWallJumpLocked`) **VÀ** game có cài đặt tự động quay mặt khi nhảy tường (`doTurnOnWallJump = true`) $\rightarrow$ Dùng lệnh `return;` để ngắt không cho chuột bẻ mặt nhân vật.
   - *Mục đích:* Tránh lỗi hình ảnh (Bug Animation) khi người chơi bật tường văng ra một hướng nhưng lại di chuột sang hướng ngược lại làm nhân vật bị xoay lật liên tục trên không trung.

### Kiểm tra collider
1. Tác giả dùng: `Physics2D.OverlapBox(Tâm_hình_chữ_nhật, Kích_thước, Góc_xoay, Layer_cần_kiểm_tra)` thay vì `OnCollisionEnter2D` vì khi dùng `OnCollisionEnter2D` sẽ tạo ra các lỗi khó chịu và còn dễ bị lỗi nhảy vô hạn nên dùng `Physics2D.OverlapBox` chỉ để vẽ cái khung **hình chữ nhật mỏng** để kiểm tra thôi là được rồi không cần cái phức tạm như `OnCollisionEnter2D`.
2. Tác giả cho kiểm tra khi ==không lướt==, ==không nhảy==
3. Kiểm tra mặt đất nếu: **Physics2D.OverlapBox** vẽ từ tâm vị trí kiểm tra mặt đất (**groundCheckPoint**) và kích thước của hộp là **groundCheckSize** không *xoay* cũng như layer là mặt đất (**groundLayer**) và không nhảy thì cho **LastOnGroundTime** tức cái thời gian châm chước về thời gian ban đầu **coyoteTime** của file dữ liệu (kiểu phục hồi thời gian châm chước)
4. Kiểm tra tường phải nếu: **Physics2D.OverlapBox** vẽ từ tâm vị trí kiểm tra tường đằng trước (**frontWallCheckPoint**) với kích thước từ kiểm tra tường (**wallCheckSize**), không *xoay* cũng như layer là mặt đất (**groundLayer**) và nhân vật đang hướng sang bên phải hoặc **Physics2D.OverlapBox** vẽ từ tâm vị trí kiểm tra tường đằng sau (**backWallCheckPoint**) với kích thước từ kiểm tra tường (**wallCheckSize**), không *xoay* cũng như layer là mặt đất (**groundLayer**) và nhân vật không hướng sang bên phải (ý là hướng sang trái vì có dấu not !) và không nhảy tường (**IsWallJumping**) thì cho thời gian nhận biết lần cuối người chơi nhảy tường bên phải (**LastOnWallRightTime**) thành dữ liệu **coyoteTime** (tức là đặt lại thời gian châm chước)
5. Kiểm tra tường trái nếu: **Physics2D.OverlapBox** vẽ từ tâm vị trí kiểm tra tường đằng trước (**frontWallCheckPoint**) với kích thước từ kiểm tra tường (**wallCheckSize**), không *xoay* cũng như layer là mặt đất (**groundLayer**) và nhân vật không hướng sang bên phải hoặc **Physics2D.OverlapBox** vẽ từ tâm vị trí kiểm tra tường đằng sau (**backWallCheckPoint**) với kích thước từ kiểm tra tường (**wallCheckSize**), không *xoay* cũng như layer là mặt đất (**groundLayer**) và nhân vật hướng sang bên phải và không nhảy tường (**IsWallJumping**) thì cho thời gian nhận biết lần cuối người chơi nhảy tường bên trái (**LastOnWallLeftTime**) thành dữ liệu **coyoteTime** (tức là đặt lại thời gian châm chước)
6. CHưa xong vì vẫn phải kiểm tra xem có đủ điều kiện và thời gian rời tường không bằng cách dùng hàm toán học ==Max== lấy **LastOnWallTime** và gán cho nó giá trị lớn nhất khi so sánh **LastOnWallLeftTime** và **LastOnWallRightTime** để cho biết rằng đang chạm tường do làm 2 hàm kiểm tra tường trái và phải thì nếu nhân vật chạm tường trái nhưng không chạm phải thì lại thành chạm tường true và false à? Quá vô lý nên hàm này dùng để nếu số bên nào lớn hơn thì tức là đang chạm tường. Ngắn gọn dễ hiểu.
### Kiểm tra nhảy
1. Nếu đang nhảy (**IsJumping**) và đạt đỉnh (**RB.velocity.y < 0**) thì tắt nhảy (**IsJumping**) và nếu không nhảy tường (**IsWallJumping**) thì cho rơi sau nhảy (**isJumpFalling**) bằng **true**.
   Lý do **chỉ bật `_isJumpFalling` khi không phải nhảy tường** — vì nhảy tường dùng cơ chế riêng, nếu bật thì sẽ làm lỗi trọng lực và animation
2. Nếu nhảy tường và thời gian tổng trừ cho thời gian đã nhảy (lưu thời gian tổng lúc bắt đầu nhảy để xem đã nhảy được bao lâu rồi) mà lớn hơn thời gian sau khi nhảy tường từ 0.1 -1.5 tùy dev chọn trong khoảng đó (**wallJumpTime**) thì tắt nhảy tường (**IsWallJumping** = false;)
3. Chạm đất còn hiệu lực (**LastOnGroundTime** > 0) và không nhảy và không nhảy tường thì tắt nhảy ngắn (**isJumpCut**) đồng thời tắt đang rơi sau nhảy (**isJumpFalling**) tức hàm này là reset trạng thái khi chạm đất và vừa trượt khỏi mép mặt đất.
4. Nếu không lướt (**IsDashing**) thì nếu có thể nhảy (**CanJump()**) và thời gian châm chước nút bấm vẫn còn (**LastPressedJumpTime** > 0) thì bật nhảy (**IsJumping**), tắt nhảy tường (**IsWallJumping**), tắt nhảy ngắn (**isJumpCut**), tắt rơi sau nhảy (**isJumpFalling**) và thực hiện nhảy (**Jump()**) nếu ngược lại có thể nhảy tường (**CanWallJump()**) và thời gian châm chước nút bấm vẫn còn (**LastPressedJumpTime** > 0) thì tắt nhảy (**IsJumping**), bật nhảy tường (**IsWallJumping**), tắt nhảy ngắn (**isJumpCut**), tắt rơi sau nhảy (**isJumpFalling**), gán thời gian nhảy tường (**wallJumpStartTime**) bằng tổng thời gian bộ đếm lúc đó (**Time.time**), xem nhân vật nhảy tường hướng mặt sang trái hay phải (**lastWallJumpDir**) bằng nếu đang chạm vào tường bên phải (**LastOnWallRightTime** > 0) thì nhảy văng sang bên trái nếu không thì chạm bên trái thì nhảy văng bên phải (**toán tử ba ngôi**: (`LastOnWallRightTime > 0) ? -1 : 1`)  và thực hiện Nhảy tường (**WallJump**) có tham số là kiểm tra nhảy tường hướng mặt trái hay phải (**lastWallJumpDir**).
### Kiểm tra lướt
1. Nếu có thể lướt (**CanDash()**) và thời gian châm chước nút bấm vẫn còn (**LastPressedDashTime** > 0) thì đóng băng thời gian (**Sleep**) theo bao lâu từ dữ liệu **dashSleepTime**, nếu phím đầu vào tương đương hướng di chuyển (**moveInput**) khác với đứng im (**vector2.zero** là biểu thị cho tọa độ 0, 0 nên được coi là đứng im) thì hướng lướt (**lastDashDir**) = phím đầu vào tương đương hướng di chuyển (**moveInput**) ngược lại hướng lướt (**lastDashDir**) bằng nếu nhân vật đang hướng sang bên phải (**IsFacingRight**) thì hướng lướt sang phải (**vector2.right** là biểu thị cho tọa độ 1, 0) ngược lại lướt sang trái (**vector2.left** là biểu thị cho tọa độ -1, 0). 
   Bật lướt (**IsDashing**)
   Tắt nhảy (**IsJumping**)
   Tắt nhảy tường (**IsWallJumping**)
   Tắt nhảy ngắn (**isJumpCut**)
   Chạy xử lý song song (**StartCoroutine**) lướt (**StartDash**) theo hướng của nhân vật (**lastDashDir**)
### Kiểm tra bám tường
1. Nếu có thể bám tường (**CanSlide()**) và đang bám tường trái (**LastOnWallLeftTime**) > 0 và đang bấm sang trái (**moveInput.x** < 0)  hoặc đang bám tường phải (**LastOnWallRightTime** > 0) và đang bấm sang phải (**moveInput.x** > 0) thì bật bám tường (**IsSliding**) ngược lại tắt bám tường (**IsSliding**).
### Trọng lực
1. Nếu không trong giai đoạn lướt (**isDashAttacking**) thì nếu đang bám tường (**IsSliding**) thì để trọng lực bằng 0 để không tác dụng (**SetGravityScale(0)**) 
2. Ngược lại nếu đang rơi (**RB.velocity.y** < 0) và đang nhấn phím xuống (**moveInput.y** < 0) thì cho trọng lực (**SetGravityScale**) bằng lấy độ lớn của lực hút (**gravityScale**) nhân với rơi nhanh bao nhiêu (**fastFallGravityMult**) cũng như giới hạn tốc độ rơi (**velocity**) bằng vận tốc ngang (**RB.velocity.x**) với dùng hàm toán học max lấy số lớn hơn của vận vận tốc dọc (**RB.velocity.y**) và giá trị âm của tốc độ rơi nhanh tối đa (**maxFastFallSpeed**)
3. Ngược lại nếu đang nhả nút nhảy sớm (**isJumpCut**) thì cho trọng lực (**SetGravityScale**) bằng lấy độ lớn của lực hút (**gravityScale**) nhân với hệ số tăng trọng lực khi người chơi buôn nút nhảy (**jumpCutGravityMult**) cũng như giới hạn vận tốc rơi (**velocity**) bằng vận tốc ngang (**RB.velocity.x**) với dùng hàm toán học max lấy số lớn hơn của vận vận tốc dọc (**RB.velocity.y**) và giá trị âm của tốc độ rơi tối đa (**maxFallSpeed**)
4. Ngược lại nếu đang nhảy (**IsJumping**) hoặc đang nhảy tường (**IsWallJumping**) hoặc đang rơi sau nhảy (**isJumpFalling**), và trị tuyệt đối (**Mathf.Abs**) của vận tốc dọc (**RB.velocity.y**) < ngưỡng vận tốc tại gần đỉnh cú nhảy (**jumpHangTimeThreshold**) thì cho trọng lực (**SetGravityScale**) bằng lấy độ lớn của lực hút (**gravityScale**) nhân với hệ số giảm trọng lực (**jumpHangGravityMult**)
5. Ngược lại nếu vận tốc dọc nhỏ hơn 0 thì cho trọng lực bằng (**SetGravityScale**) lấy độ lớn của lực hút (**gravityScale**) nhân với hệ số trọng lực khi rơi (**fallGravityMult**) cũng như giới hạn vận tốc rơi (**velocity**) bằng vận tốc ngang (**RB.velocity.x**) với dùng hàm toán học max lấy số lớn hơn của vận vận tốc dọc (**RB.velocity.y**) và giá trị âm của tốc độ rơi tối đa (**maxFallSpeed**)
6. Ngược lại cho trọng lực (**SetGravityScale**) bằng lấy độ lớn của lực hút (**gravityScale**)
7. Ngược lại đang trong giai đoạn lướt thì đặt trọng lực bằng 0 (**SetGravityScale(0**)) vì khi lướt không có trọng lực và chỉ trợ lại bình thường sau khi giai đoạn lướt kết thúc. Phần này là ngược lại của 1.
#### Hàm fixedUpdate xử lý chạy
1. Nếu không lướt
2. Nếu đang nhảy tường thì thực hiện **Run** với tham số mức độ kiểm soát của người chơi (**wallJumpRunLerp**)
3. Ngược lại thì thực hiện **Run** với tham số là **1**
4. Ngược lại thì (của 1) đang trong giai đoạn lướt (**isDashAttacking**) thực hiện Run với tham số làm chậm khả năng bẻ lái khi hãm phanh (**dashEndRunLerp**)
5. Nếu đang bám tường (**IsSliding**) thì thực hiện **Slide()**

### Nạp thời gian đầu vào
1. Hàm **OnJumpInput()** Xử lý bộ nhớ đệm phím nhảy. Khi người chơi nhấn phím nhảy sớm lúc đang rơi gần chạm đất, bộ đếm (**LastPressedJumpTime**) sẽ được gán bằng thời gian châm chước (**jumpInputBufferTime**) để tự động kích hoạt nhảy ngay khi chạm đất.
2. Hàm **OnJumpUpInput()** nếu đang nhảy mà buông phím nhảy sớm (**CanJumpCut()**) hoặc đang nhảy tường mà buông phím sớm (**CanWallJumpCut()**) thì bật cờ nhảy ngắn (**isJumpCut**)
3. Hàm **OnDashInput()** Tương tự như nhảy, gán bộ đếm châm chước nhấn lướt sớm (**LastPressedDashTime**) bằng thời gian châm chước lướt (**dashInputBufferTime**)
### Hàm chung
1. Hàm đặt trọng lực (**SetGravityScale**) với public, void có tham số đầu vào là **scale** kiểu float: đặt trọng lực (**RB.gravityScale**) bằng tham số đầu vào **scale**
2. Hàm ngủ (**Sleep**) với private, void có tham số đầu vào **duration** kiểu float: Cho xử lý song song (**StartCoroutine**) với tên **PerformSleep** (dùng **nameof**) , tham số đầu vào **duration**
3. Hàm thực thi xử lý song song (**IEnumerator**) có tên **PerformSleep** có tham số đầu vào **duration**: cho thời gian trong game bằng 0 (**Time.timeScale**) để dừng thời gian rồi tạo luồng xử lý song song (**yield**) với luồng thời gian thực tế (**WaitForSecondsRealtime**) dùng tham số đầu vào **duration**, cuối cùng đặt thời gian trong game (**Time.timeScale**) về 1 để cho thời gian trôi. 
   => hàm này là chắc chắn dùng cho lướt vì theo như đã đọc thì chỉ có lướt mới chơi dừng thời gian của game và để người chơi vẫn thao tác được mà thôi (tức lấy thời gian thực, thời gian không bị ảnh hưởng bởi game)
### Hàm phương thức di chuyển
1. Hàm **Run**: tính hướng di chuyển và vận tốc mong muốn, lấy tốc độ mục tiêu (**targetSpeed**) bằng nút bấm di chuyển ngang (**moveInput.x**) nhân với tốc độ tối đa (**runMaxSpeed**) muốn người chơi đạt được, sau đó ==điều chỉnh== tốc độ mục tiêu bằng nội suy tuyến tính (**Mathf.Lerp**) bằng cách hiệu chỉnh trong khoảng giữa giá trị vận tốc hướng ngang (**RB.velocity.x**) với tốc độ mục tiêu (**targetSpeed**) theo tỉ lệ **lerpAmount** (luôn trong khoảng 0 và 1), cũng như xác định xem người chơi có quyền di chuyển khi lướt với nhảy tường. 
   Ví dụ tước quyền di chuyển dùng nội suy tuyến tính (lerp): 
	1. vận tốc ngang: 10
	2. vận tốc mục tiêu: 30
	3. tỉ lệ tuyến tính: 0
	$\rightarrow$ Theo công thức: $a + (b - a) \times t$ thì $10 + (30 - 10) \times 0$ = 10
	Rồi sang [[#^ap-dung-luc-va-lat-anh|áp dụng lực]] dùng kết quả là 10 đó ở **speedDif** trừ cho chính vận tốc ngang thì ra ==0==, xong lại nhân với gia tốc lại bằng ==0==, rồi áp dụng lực thì thế nào tự hiểu. ĐÓ là cách tước quyền di chuyển của người chơi. 
	$\rightarrow$ ==Thực ra chỉ là tính toán logic thông thường thôi không có gì đặc biệt cả, chỉ là cái vụ tước quyền di chuyển đơn giản là không cho áp dụng lực (tức là 0 ấy). Không phải tắt di chuyển hay gắn cờ gì hết. Phải công nhận là dùng tính toán hay thiệt.==
#### Tính toán gia tốc
1. Thêm biến gia tốc (**accelRate**) kiểu float
2. Nếu chạm đất còn hiệu lực (**LastOnGroundTime** > 0) thì gia tốc (**accelRate**) bằng nếu giá trị tuyệt đối của tốc độ mục tiêu (**targetSpeed**) tức đang di chuyển mà lớn hơn 0.01f thì bằng lực tăng tốc thực tế (**runAccelAmount**) ngược lại bằng lực giảm tốc thực tế (**runDeccelAmount**)
3. Ngược lại gia tốc (**accelRate**) bằng nếu giá trị tuyệt đối của tốc độ mục tiêu (**targetSpeed**) tức đang di chuyển mà lớn hơn 0.01f thì bằng lực tăng tốc thực tế nhân với khả năng chuyển hướng trên không (**accelInAir**) ngược lại bằng lực giảm tốc thực tế (**runDeccelAmount**) nhân với khả năng hãm phanh trên không (**deccelInAir**)
#### Tăng cường gia tốc bẻ lái tại đỉnh cú nhảy
1. Nếu đang nhảy (**IsJumping**) hoặc nhảy tường (**IsWallJumping**), hoặc rơi sau nhảy (**isJumpFalling**) và giá trị tuyệt đổi của vẫn tốc dọc (**RB.velocity.y**) < xác định xem đã gần đỉnh cú nhảy chưa (**jumpHangTimeThreshold**) thì gia tốc (**accelRate**) nhân bằng gia tốc lở lửng cho phép bẻ lái trên không (**jumpHangAccelerationMult**) cũng như vận tốc mục tiêu (**targetSpeed**) nhân bằng tốc đa lơ lửng tối đa để tạo cảm giác bật nhảy xa hơn (**jumpHangMaxSpeedMult**)
#### Bảo toàn động lực
1. Nếu tính năng bảo toàn động lượng được bật (**doConserveMomentum**) ==VÀ== vận tốc ngang hiện tại lớn hơn tốc độ chạy tối đa (`Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed)`) ==VÀ== nhân vật đang bay cùng hướng với phím bấm (`Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed)`) ==VÀ== người chơi đang bấm giữ phím di chuyển (`Mathf.Abs(targetSpeed) > 0.01f`) ==VÀ== nhân vật đang ở trên không (`LastOnGroundTime < 0`) $\rightarrow$ Thì gán gia tốc bằng 0 (`accelRate = 0`). Giúp **ngăn chặn việc game tự động hãm phanh** người chơi khi họ đang bay nhanh hơn tốc độ bình thường (ví dụ như lực quán tính dôi ra sau khi Lướt).
#### Vẫn trong khối phương thức di chuyển ^ap-dung-luc-va-lat-anh
1. TÍnh toán khác biệt giữa vận tốc hiện tại và vận tốc mong muốn (**speedDif**) bằng lấy vận tốc mục tiêu (**targetSpeed**) trừ cho vận tốc di chuyển ngang (**RB.velocity.x**)
2. Tính di chuyển (**movement**) bằng lấy vận tốc khác biệt (**speedDif**) nhân với gia tốc (**accelRate**)
3. Thêm lực đẩy (**RB.AddForce**): lấy di chuyển ngang nhân với vector hướng phải (`Vector2.right` tức tọa độ 1,0) để biến con số thành một lực đẩy vật lý (có hướng trái hoặc phải tùy thuộc dấu âm dương của `movement`). Sau đó dùng hàm `AddForce` để ép lực này lên nhân vật một cách liên tục (`ForceMode2D.Force`)
4. Hàm **Turn**: ==Lưu tỷ lệ và lật hình ảnh người chơi dọc theo trục x==. Cụ thể: Khai báo biến **scale** kiểu `Vector3` để hứng lấy tỉ lệ thu phóng hiện tại (**transform.localScale**).  Sau đó nhân giá trị `scale.x` với `-1` rùi gán ngược lại biến `scale` vào (**transform.localScale**) để lật ảnh. Cuối cùng, đảo ngược trạng thái của cờ quay mặt (`IsFacingRight = !IsFacingRight`).
### Hàm nhảy
#### Thực hiện nhảy
1. Hàm **Jump** kiểu void, private: Để đảm bảo không gọi nhảy nhiều lần thì gán các biến châm chước (**LastPressedJumpTime** và **LastOnGroundTime**) về 0 để reset bộ đếm trước tránh gọi nhiều lần nhảy.
2. Khai báo biến `force` kiểu float lấy lực đẩy lên thực tế **jumpForce**
3. Nếu đang rơi (**RB.velocity.y** < 0) thì cho lực (**force**) trừ vận tốc rơi đang là âm(**RB.velocity.y**). Ví dụ lực nhảy gốc là `10` và tốc độ rơi là `-3` nếu chỉ cộng `10` lực nhảy thì sẽ tốn `3` lực bù lại nên lực nhảy thực tế chỉ là `7` $\rightarrow$ dùng  `force -= RB.velocity.y` sẽ thành cộng thêm `3` thành `13` bù trừ cho tốc độ rơi `-3`.
4. Thêm lực từ hướng lên (**Vector2.up**) nhân với lực (**force**) rồi áp lực xung kích (**ForceMode2D.Impulse**) để nhảy lên trong 1 khung hình luôn.
#### Thực hiện nhảy tường
1. Hàm **nhảy tường** với tham số đầu vào là **dir** kiểu int: 
   reset các biến `LastPressedJumpTime`, `LastOnGroundTime`, `LastOnWallRightTime`, `LastOnWallLeftTime` về 0 để tránh việc gọi nhảy tường nhiều lần từ 1 lần nhấn.
2. Khai báo force kiểu `vector2` bằng new lấy dữ liệu từ lực thực tế khi nhảy tường ngang (**wallJumpForce.x**) và lực thực tế khi nhảy tường dọc (**wallJumpForce.y**)
3. CHo **force.x** nhân bằng **dir** để áp dụng lực theo hướng ngược lại của tường
4. Nếu hướng của vận tốc ngang (**velocity.x**) khác với hướng của **force.x** thì **force.x** trừ bằng vận tốc ngang (==trừ cho trừ thành cộng==), kiểu nếu đâm vào tường ở ngược hướng bay ra thì bù lại lực thêm lực `x` để cho bật tường tiếp (cái trò trèo lên từ 2 tường song song ấy) 
5. Nếu người chơi đang rơi (**RB.velocity.y**< 0) thì **force.y** trừ bằng vận tốc dọc (**velocity.y**), (==trừ cho trừ thành cộng==). Nếu đang lỡ trớn rớt xuống đáy, thì bù thêm lực y. Kết quả: Cú nhảy tường **LUÔN LUÔN** nảy ra đúng một quỹ đạo cong hình parabol hoàn hảo, bất chấp việc trước đó bạn đang rơi hay bay như thế nào!
6. THêm lực từ force rồi áp dụng lực xung kích
#### Thực hiện quay mặt khi nhảy tường
 1. Nếu xoay mặt nhân vật theo hướng nhảy tường (**doTurnOnWallJump**) được bật thì gọi hàm kiểm tra ==CheckDirectionToFace== với tham số đầu vào là `dir > 0` (chính chỗ này đã là một giá trị bool rồi rồi, ví dụ: nếu dir = 1 thì chỗ này sẽ là 1 > 0 tức là true ngược lại thì là false vậy là tham số đầu vào hợp lệ rồi. Lưu ý: ==Trong C#, bất kỳ phép so sánh nào (`>`, `<`, `==`, `!=`...) đều tự động trả về `true` hoặc `false`==)

#### Thực hiện Nhảy đôi (Double Jump)
1. **Khởi tạo**: Dùng biến private `_bonusJumpsLeft` kiểu int để theo dõi số lần nhảy đôi còn lại.
2. **Nạp lại (Refill)**: Trong mô hình State Machine, khi nhân vật chuyển sang trạng thái `PlayerState.Grounded` (Chạm đất) qua hàm `TransitionToState`, biến `_bonusJumpsLeft` sẽ tự động được reset bằng `Data.bonusJumpAmount`.
3. **Điều kiện (`CanDoubleJump`)**: Chỉ được phép nhảy khi số lượt `_bonusJumpsLeft > 0`, KHÔNG chạm đất (`LastOnGroundTime <= 0`) và KHÔNG đang nhảy tường (`!IsWallJumping`).
4. **Thực thi (`HandleJumpChecks`)**: Khi gọi hàm `Jump()` thông qua nhánh điều kiện nhảy đôi, trừ đi 1 lượt (`_bonusJumpsLeft--`) rồi mới chuyển state sang `PlayerState.Jumping` để áp lực AddForce.
 
### Hàm lướt
- Làm cái xử lý song song (**IEnumerator**) tên **StartDash** với tham số đầu vào là **dir** kiểu vector2
1. Reset các biến đếm như **LastOnGroundTime** và **LastPressedDashTime** về 0
2. Khai báo bắt đầu kiểu float (**startTime**) bằng thời gian hiện tại của game (**Time.time**) giây
3. Trừ số lần lướt `dashesLeft--`
4. Bật giai đoạn đầu của lướt `isDashAttacking = true` tức là xung kích
5. CHo lực hút bằng 0 (**SetGravityScale(0)**), tức là là tắt trọng lực
6. Trong khi thời gian hiện tại của game (**Time.time**) - cho thời gian bắt đầu (**startTime**) nhỏ hơn hoặc bằng quãng thời gian duy trì lướt (**dashAttackTime**) thì vận tốc (**velocity**) bằng hướng **dir** nhân với tốc độ lướt (**dashSpeed**) đồng thời tạm dừng vòng lặp và chờ đến khung hình tiếp theo `yield return null`
7. Gán bắt đầu (**startTime**) bằng thời gian hiện tại của game (**Time.time**)
8. Tắt giai đoạn lướt ban đầu `isDashAttacking = false`
9. Đặt lại trọng lực từ độ lớn trọng lực `SetGravityScale(Data.gravityScale)`
10. Gán vận tốc **velocity** bằng tốc độ kết thúc lướt (`dashEndSpeed`) nhân với hướng `dir`
11. Trong khi thời gian hiện tại của game (**Time.time**) - cho thời gian bắt đầu (**startTime**) nhỏ hơn hoặc bằng thời gian hãm phanh sau khi lướt thì chờ đến khung hình tiếp theo `yield return null`
12. Tắt đang lướt để game biết là lướt xong rồi

- Làm thêm cái xử lý song song để nạp lại lướt, tên RefillDash với tham số đầu vào là amount kiểu int
1. Bật hồi số lần lướt
2. Khai báo thời gian chờ hồi lấy thời gian từ hồi chiêu lướt `yield return new WaitForSeconds(Data.dashRefillTime);`
3. Tắt hồi số lần lướt
4. Cập nhật lại số lần lướt (`_dashesLeft`). Dùng hàm `Mathf.Min` để đảm bảo số lần lướt được cộng thêm 1 nhưng **không bao giờ vượt mức tối đa** cho phép (`Data.dashAmount`).
### Hàm di chuyển khác
1. 1. Hàm **Slide()** xử lý trượt tường theo trục Y (có thể độ chế lại thành leo tường nếu gắn thêm input của người chơi).
2. 1. Tính **Độ chênh lệch vận tốc (`speedDif`)** bằng cách lấy Tốc độ trượt (**slideSpeed**) mong muốn trừ đi Vận tốc dọc (**velocity.y**).
3. Khai báo lực di chuyển **(`movement`)** kiểu float, và tính bằng cách lấy Độ chênh lệch (**speedDif**) nhân với Gia tốc trượt (**slideAccel**).
4. Di chuyển bằng toán tử giới hạn giá trị trong một khoảng (**tức giới hạn lực đẩy**): Di chuyển (**movement**) là *giá trị trả về*, âm giá trị tuyệt đối của độ chênh lệch vận tốc `-Mathf.Abs(speedDif)` nhân với 1 chia với thời gian cố định tính bằng giây (là 50) `1 / Time.fixedDeltaTime` là *giá trị min*, giá trị tuyệt đối của độ chênh lệch vận tốc `Mathf.Abs(speedDif)` nhân cho 1 chia với thời gian cố định tính bằng giây (là 50) `1 / Time.fixedDeltaTime)` là *giá trị max*.
5. Gán lực bằng di chuyển nhân với hướng lên `RB.AddForce(movement * Vector2.up)`

#### Leo tường (Wall Climb) - Chế lại từ Slide
1. Nếu như trượt tường (Slide) chỉ có rớt xuống do vận tốc cài đặt sẵn là âm, thì Leo tường (Climb) yêu cầu đọc thêm phím nhấn lên/xuống (`moveInput.y`).
2. Trong hàm `Slide()`, nếu người chơi nhấn đi lên (`moveInput.y > 0`), biến **Tốc độ trượt mong muốn** (`speedDif`) sẽ được đổi từ `slideSpeed` (âm) sang `wallClimbSpeed` (dương).
3. Gia tốc trượt (`slideAccel`) sẽ được thay bằng `wallClimbAccel` để nhân vật "bốc" lên mạnh hơn.
4. Cuối cùng, hàm sẽ đẩy nhân vật trượt thẳng lên trên thay vì rớt xuống dưới, kết hợp với animation leo tường sẽ ra cơ chế chuẩn như các game Celeste hay Hollow Knight!

### Hàm kiểm tra
Lưu ý: ==Trong C#, bất kỳ phép logic hay so sánh nào (`>`, `<`, `>=`, `<=`, `==`, `!=`, `!`, `&&`, `||`, `^`,...) đều tự động nhả ra kết quả `true` hoặc `false` nên chỉ cần 1 dòng `return` là xong. Nhưng nếu cần chèn thêm tác vụ phụ (viết log, kích hoạt hiệu ứng, âm thanh, phát sự kiện UI,...) thì cấu trúc phân nhánh IF-ELSE vẫn là chân lý.==
1. **Kiểm tra hướng nhìn** (**CheckDirectionToFace**): kiểu void, public và có tham số đầu vào là đang di chuyển sang phải (**isMovingRight**) kiểu bool. Nếu đang di chuyển sang phải (**isMovingRight**) khác với đang nhìn sang bên phải (**IsFacingRight**) thì thực hiện hàm **Turn()** hàm lật ảnh.
   
2. **Kiểm tra có thể nhảy không** (**CanJump**): bằng cách trả về true khi chạm đất còn hiệu lực (**LastOnGroundTime** > 0) và không đang nhảy (**!IsJumping**) ngược lại nếu 1 trong 2 là false hay cả 2 giá trị đó false thì trả về false.
   
3. **Kiểm tra có thể nhảy tường không (`CanWallJump`)**: bằng cách trả về `true` khi thỏa mãn TẤT CẢ các điều kiện: vừa nhấn nhảy (**LastPressedJumpTime** > 0), VÀ vẫn trong thời gian châm chước bám tường (**LastOnWallTime** > 0), VÀ KHÔNG chạm mặt đất (**LastOnGroundTime** <= 0), VÀ (KHÔNG đang nhảy tường (!**IsWallJumping**) HOẶC đang bám tường phải mà lần nhảy trước là hướng sang trái HOẶC đang bám tường trái mà lần nhảy trước là hướng sang phải). Hàm phức tạp này giúp nhân vật không bị lỗi nhảy đúp liên tục trên cùng một vách tường. Ngược lại nếu 1 trong các điều kiện là `false` thì trả về `false`.
    
4. **Kiểm tra có thể ngắt nhảy không (`CanJumpCut`)**: bằng cách trả về `true` khi đang trong trạng thái nhảy (**IsJumping**) VÀ cơ thể vẫn đang bay lên trên (**RB.velocity.y** > 0). Ngược lại trả về `false` (nghĩa là nếu đã bắt đầu rớt xuống thì không cho ngắt nhảy nữa).
    
5. **Kiểm tra có thể ngắt nhảy tường không (`CanWallJumpCut`)**: tương tự hàm ngắt nhảy thường, trả về `true` khi đang trong trạng thái nhảy tường (**IsWallJumping**) VÀ cơ thể vẫn đang bay lên trên (**RB.velocity.y** > 0). Ngược lại trả về `false`.
    
6. **Kiểm tra có thể lướt không (`CanDash`)**:
    
    - _Bước 1:_ Nếu KHÔNG đang lướt (!**IsDashing**) VÀ số lần lướt chưa đạt mức tối đa (**dashesLeft < Data.dashAmount**) VÀ chạm đất còn hiệu lực (**LastOnGroundTime** > 0) VÀ chưa trong tiến trình hồi lướt (!**_dashRefilling**), thì kích hoạt xử lý song song (`StartCoroutine`) hàm **RefillDash** để tự nạp lại lướt.
    - _Bước 2:_ Trả về `true` nếu số lần lướt còn lại lớn hơn 0 (**dashesLeft** > 0), ngược lại nếu đã hết số lần lướt thì trả về `false`.
      
7. **Kiểm tra có thể trượt tường không (`CanSlide`)**: bằng cách trả về `true` khi đang bám tường (**LastOnWallTime** > 0) VÀ KHÔNG đang nhảy (!**IsJumping**) VÀ KHÔNG đang nhảy tường (!**IsWallJumping**) VÀ KHÔNG đang lướt (!**IsDashing**) VÀ KHÔNG chạm mặt đất (**LastOnGroundTime** <= 0). Ngược lại nếu 1 trong các vế này vi phạm (ví dụ đang lướt hoặc đang chạm đất) thì trả về `false`.
### Hàm editor
1. **Vẽ vùng kiểm tra va chạm** (**OnDrawGizmosSelected**): kiểu void, private. Đây là một hàm đặc biệt có sẵn của Unity, chỉ hoạt động trong màn hình Scene của Editor (không chạy trong game thật) để hiển thị hình học không gian hỗ trợ quan sát. Cụ thể:
    - Đổi bút vẽ sang màu xanh lá (`Color.green`) 
    - Vẽ một khung chữ nhật rỗng (`DrawWireCube`) tại vị trí kiểm tra chạm đất (**groundCheckPoint**) và theo kích cỡ của **groundCheckSize**.
    - Đổi bút vẽ sang màu xanh dương (`Color.blue`) 
    - Vẽ tiếp hai khung chữ nhật rỗng tại vị trí kiểm tra chạm tường trước (**frontWallCheckPoint**) và chạm tường sau (**backWallCheckPoint**) theo kích cỡ của **wallCheckSize**.
    - _Mục đích:_ Giúp người lập trình nhìn thấy trực quan "hộp va chạm" vô hình của nhân vật để dễ dàng tinh chỉnh kích thước và vị trí cho chuẩn xác.