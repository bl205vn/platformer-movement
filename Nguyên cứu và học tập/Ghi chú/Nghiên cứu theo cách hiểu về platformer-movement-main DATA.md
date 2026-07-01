---
Bí danh:
  - Nghiên cứu
tags:
  - Dự_án_tốt_nghiệp
Ngày: 2026-06-08
Trạng thái: 🟢Đã xong
Liên quan:
  - "[[Nghiên cứu theo cách hiểu về platformer-movement-main DATA]]"
  - "[[Hiểu khi xem video về Platformer của kênh Dawnosaur]]"
  - "[[Các hàm tính toán trong unity]]"
---
# Mục lục
### Tên hàm

1. [[#^79135a|Gravity/trọng lực]]
2. [[#^4d4be9|Run/chạy]]
3. [[#^a54fe4|Jump/nhảy]]
4. [[#^ac3d3a|Both Jumps/cả 2 lực nhảy]]
5. [[#^8ab04e|Wall Jump/nhảy tường]]
6. [[#^510ca6|Slide/Trượt tường]]
7. [[#^483f07|Assists/Hỗ trợ]]
8. [[#^cf0aca|Dash/Lướt]]
9. [[#^aeced9|Nhảy đôi]]
10. [[#^2ba0f6|Leo tường]]
### Các công thức toán học

1. [[#^tinh-toan-do-lon-trong-luc| Tính toán độ lớn trọng lực]]
2. [[#^tinh-toan-gravity-scale-cua-rigidbody| Tính toán gravity scale của rigidbody]]
3. [[#^tinh-toan-luc-gia-toc-va-giam-toc|Tính toán lực gia tốc và giảm tốc]]
4. [[#^tinh-toan-luc-nhay|Tính toán lực nhảy]]
5. [[#^gioi-han-gia-tri-hop-le|Giới hạn giá trị hợp lệ]]
## Tên hàm:

1. ==Gravity/trọng lực==  ^79135a
   
	- **gravityStrength** ==(tính tự động)==: Lực hướng xuống (trọng lực).
	  
	- **gravityScale** ==(tính tự động)==: Độ lớn trọng lực.
				Cũng là giá trị mà `rigidbody2D.gravityScale` (chỉnh trong **ProjectSettings/Physics2D**, mặc định là ==-9.81== nên chỉnh thành ==-30==) của người chơi được thiết lập.
				
	- **fallGravityMult** (*Hệ số nhân*): nhân cho **gravityScale** để làm cho ==lực rơi== mạnh mẽ hơn.
	  
	- **maxFallSpeed**: ==Tốc độ rơi== tối đa.
	  
	- **fastFallGravityMult** (*Hệ số nhân*): ==rơi nhanh bao nhiêu== khi nhấn phím xuống.
	  
	- **maxFastFallSpeed**: ==Tốc độ rơi nhanh== tối đa.
	  
2. ==Run/chạy== ^4d4be9
   
	- **runMaxSpeed**: ==Tốc độ chạy== tối đa muốn đạt.
	  
	- **runAcceleration**: ==Tốc độ chạy tăng ==tốc lên *tối đa*, có thể đặt bằng `runMaxSpeed` để tăng tốc tức thời hoặc bằng *0* để không tăng tốc.
	  
	- **runAccelAmount** ==(tính tự động)==: Lực *tăng* tốc thực tế.
	  
	- **runDecceleration**: Tốc độ *giảm tốc* từ tốc độ hiện tại, có thể đặt bằng `runMaxSpeed` để giảm tốc tức thời hoặc bằng *0* để không giảm tốc.
	  
	- **runDeccelAmount** ==(tính tự động)==: Lực *giảm* tốc thực tế.
	  
	- **accelInAir** (*Hệ số nhân từ 0 đến 1*): Cho người chơi ==chuyển hướng trên không== mượt cỡ nào, ==0== là không cho chuyển, ==1== là cho chuyển.
	  
	- **deccelInAir** (*Hệ số nhân từ 0 đến 1*): Cho phép người chơi ==hãm phanh trên không== không, ==0== là cứ để quán tính đẩy đi, ==1== là cho phép người chơi buôn phím định hướng để rơi xuống tại chỗ nhả phím đó.
	  
	- **doConserveMomentum** (*cờ/flag*): ==*Bảo toàn*== động lượng/đà. Đảm bảo không làm cho chức năng dash bị ==xung đột== với chạy/run (bị giảm tốc khi vận tốc vượt quá `runMaxSpeed`).
	  
3. ==Jump/nhảy== ^a54fe4
   
	- **jumpHeight**: *Độ cao* cú nhảy.
	  
	- **jumpTimeToApex**: *Thời gian* giữa lúc áp dụng lực nhảy và đạt độ cao mong muốn. *Giá trị này cũng kiểm soát trọng lực và lực nhảy của người chơi*.
	  
	- **jumpForce** ==(tính tự động)==: Lực đẩy lên thực tế.
	  
4. ==Both Jumps/cả 2 lực nhảy== (ko phải nhảy 2 lần mà là áp lực vào nhảy thường và nhảy tường, nói chung là ==tạo cảm giác nhảy==) ^ac3d3a
   
	- **jumpCutGravityMult** (*Hệ số nhân*): Để ==tăng trọng lực== nếu người chơi nhả nút nhảy trong khi nhân vật vẫn đang nhảy.
	  
	- **jumpHangTimeThreshold**: Xác định xem người chơi đã *gần đạt đỉnh cú nhảy* hay chưa bằng cách **xem vận tốc y gần bằng 0** hay không.
	  
	- **jumpHangGravityMult** (*Hệ số nhân*): ==Giảm trọng lực== (thường < 1) khi nhân vật *đạt đỉnh cú nhảy* (vận tốc y gần bằng 0). Trọng lực *đột ngột yếu* đi từ đó tạo **cảm giác lơ lửng**.
	  
	- **jumpHangAccelerationMult** (*Hệ số nhân*): Gia tốc lơ lửng, giúp đổi hướng/bẻ lái sang trái phải khi đang trên không nhanh hơn xíu. Bổ trợ cho **accelInAir**.
	  
	- **jumpHangMaxSpeedMult** (*Hệ số nhân*): Tốc độ tối đa lơ lửng, tạo cảm giác bật nhảy xa hơn cho người chơi với giới hạn tốc độ  **runMaxSpeed** nhỉnh hơn xíu.
	  
5. ==Wall Jump/nhảy tường== ^8ab04e
   
	- **wallJumpForce**: *Lực thực tế* khi nhảy tường.
	  
	- **wallJumpRunLerp** (*từ 0 đến 1*): Khả năng kiểm soát di chuyển của người chơi khi nhảy tường, ==0== là không cho người chơi kiểm soát, ==1== là cho kiểm soát di chuyển.
	  
	- **wallJumpTime** (*từ 0 đến 1.5 giây*): Thời gian ==tước quyền== kiểm soát di chuyển.
	  
	- **doTurnOnWallJump** (*cờ/flag*): Tự quay mặt ra ==hướng ngược lại== khi nhảy tường.
	  
6. ==Slide/Trượt tường== ^510ca6
   
	- **slideSpeed**: _Tốc độ_ trượt tường mục tiêu. Khi nhân vật bám tường sẽ cố gắng giữ vận tốc này. _(Ghi chú thêm: Nếu set bằng 0 thì sẽ đứng im trên tường, nếu set số âm nhỏ thì sẽ trượt dần xuống)._
	  
	- **slideAccel**: _Gia tốc_ trượt tường, tăng/giảm tốc (hoặc phanh lại) dần dần cho đến khi vận tốc thực tế của nhân vật bằng với **slideSpeed**. _(Ghi chú thêm: Số càng to thì phanh càng gấp, bám tường càng chắc)._
	  
7. ==Assists/Hỗ trợ==  ^483f07
   
	- **coyoteTime** (từ 0.01 đến 0.5): *Thời gian châm chước* cho người chơi nhảy ==*khi đã rơi khỏi nền đất*==.
	  
	- **jumpInputBufferTime** (từ 0.01 đến 0.5):  *Thời gian châm chước* khi người chơi nhấn nút nhảy để *tự động thực hiện* ngay (kiểu khi *nhân vật đang rơi sắp chạm đất* mà người chơi bấm nút nhảy **trong thời gian châm chước** thì game sẽ tự động nhảy khi nhân vật chạm đất).
	  
8. ==Dash/Lướt== ^cf0aca
   
	- **dashAmount**: *Số lần* có thể lướt.
	  
	- **dashSpeed**: *Vận tốc bị đẩy* đi khi lướt.
	  
	- **dashSleepTime**: ==Khựng hình==, khi vừa bấm lướt game sẽ **đóng băng** trong khoảng thời gian siêu ngắn để tạo cho người chơi *cảm giác* tụ lực cực mạnh và cho người chơi thêm **tích tắc** để bấm phím *di chuyển* định hướng và **giúp game** nhận diện chính xác hơn. 
	  
	- **dashAttackTime**: Quãng *thời gian duy trì* lướt ở vận tốc **dashSpeed**, ==trọng lực sẽ bị vô hiệu hóa==.
	  
	- **dashEndTime**: Thời gian *hãm phanh sau khi lướt*.
	  
	- **dashEndSpeed**: *Giới hạn tốc độ* sau khi hãm phanh.
	  
	- **dashEndRunLerp** (*từ 0 đến1*): *Làm chậm khả năng bẻ lái* khi đang hãm phanh. (kiểu khi đang *lướt bên phải* mà người chơi nhấn phím *định hướng trái* thì hàm sẽ *làm chậm khả năng bẻ lái* để cho đúng logic vật lý).
	  
	- **dashRefillTime**: ==Hồi chiêu lướt==, *tránh người chơi spam* lướt liên tục và muốn thì có thể bỏ qua nếu dùng kiểu cho phép *đáp đất* là *hồi luôn* hoặc kiểu bắt người chơi ăn đồ *thêm lượt lướt*.
	  
	- **dashInputBufferTime** (*từ 0.01 đến 0.5*): *Thời gian châm chước* cho người chơi **bấm lướt khi lướt vẫn đang trong thời gian hồi** mà trong thời gian châm chước thì **game sẽ ghi nhớ** cho đến khi lướt hồi xong thì tự động lướt. Tương tự [[#^483f07|Assists/Hỗ trợ]].
9. ==Double Jump/Nhảy đôi== ^aeced9
   
	- **bonusJumpAmount**: Số lượng cú nhảy trên không (Nhảy đôi) cho phép. (Ví dụ set = 1 thì có thể nhảy thêm 1 lần khi đang lơ lửng).

10. ==Wall Climb/Leo tường== ^2ba0f6
   
	- **wallClimbSpeed**: Tốc độ leo lên khi bám tường (dương = lên).
	- **wallClimbAccel**: Gia tốc để leo tường. Số càng lớn thì bám/hãm phanh trên tường càng dính, giúp chuyển đổi mượt mà giữa trạng thái trượt xuống (slide) và leo lên (climb).

## Các công thức toán học:

1. Tính toán độ **lớn trọng lực** bằng công thức **(gravity = 2 * jumpHeight / timeToJumpApex^2)**: ^tinh-toan-do-lon-trong-luc
   
    Trong code: $gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex)$
    
    - Đây là lực kéo xuống **thực tế mà tác giả muốn** để nhân vật nhảy đúng độ cao `jumpHeight` trong đúng thời gian `jumpTimeToApex`. Ví dụ ra khoảng **-44.4**.
      
2. Tính toán gravity scale của` rigidbody` (ví dụ: độ lớn trọng lực tương đối so với giá trị trọng lực của Unity, xem project settings/Physics2D): ^tinh-toan-gravity-scale-cua-rigidbody
   
	- GIải thích dễ hiểu thì công thức này **tính trọng lực mà game sẽ tác dụng lên gameobject**, lý do phải tính vậy vì **Unity** không cho **set trọng lực riêng cho từng object bằng đơn vị m/s²**. Thay vào đó nó dùng `gravityScale` — một **hệ số nhân** so với trọng lực toàn cục:
     
     Công thức: $lực trọng lực thực tế của object = Physics2D.gravity.y × gravityScale$
     
	- Nên muốn object ==chịu đúng lực== **`-44.4`**, bạn phải tìm hệ số:
     
     Trong code kèm ví dụ: 
     $gravityScale = gravityStrength / Physics2D.gravity.y$
             $= -44.4 / -9.81$
             $≈ 4.53$
             
	$\Rightarrow$ Tức là nhân vật chịu trọng lực **gấp 4.53 lần** trọng lực mặc định của Unity — và kết quả thực tế sẽ ra đúng `-44.4` theo tính toán:
	
			$lực trọng lực thực tế của object = Physics2D.gravity.y × gravityScale$
							$= -9.81 × 4.53$
							$≈ -44.4$
	
	- `Physics2D.gravity.y` là ==giá trị trọng lực== của Unity, mặc định = **-9.81** (có thể đổi trong Project Settings → Physics 2D).
	  
3. ==Tính toán lực gia tốc== và ==giảm tốc khi chạy== bằng công thức: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed: ^tinh-toan-luc-gia-toc-va-giam-toc

	- ==Lực gia tốc==: $runAccelAmount = (50 * runAcceleration) / runMaxSpeed$
	- ==Lực giảm tốc==: $runDeccelAmount = (50 * runDecceleration) / runMaxSpeed$
	  
	  
	  $\Rightarrow$ `50` ở đây vì `Time.fixedDeltaTime` mặc định = 0.02s, tức `1/0.02 = 50` - tác giả hardcode luôn:))
	  
4. ==Tính toán lực nhảy== bằng công thức (initialJumpVelocity = gravity * timeToJumpApex) ^tinh-toan-luc-nhay
	  
	  $jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex$
	
5. Giới hạn giá trị hợp lệ cho các thuộc tính ^gioi-han-gia-tri-hop-le
   
	- $runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed)$
	- $runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed)$
	  
	$\Rightarrow$ Đơn giản là cho thuộc tính ***tăng tốc*** với ***giảm tốc*** thấp nhất là 0.01 và lớn nhất theo **runMaxSpeed** tránh việc người chơi đứng im.