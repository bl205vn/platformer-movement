---
Bí danh:
  - Nghiên cứu
tags:
  - Dự_án_tốt_nghiệp
Ngày: 2026-06-11
Trạng thái: 🔴Đang làm
Liên quan:
  - "[[Nghiên cứu theo cách hiểu về platformer-movement-main DATA]]"
  - "[[Nghiên cứu theo cách hiểu về platformer-movement-main tính toán]]"
  - "[[Các hàm tính toán trong unity]]"
---
# Video: https://youtu.be/KbtcEVCM7bw?si=4muwnURB5mvl9kFK
1. Chạy gồm 3 phần: Gia tốc $\Rightarrow$ Tốc độ tối đa $\Rightarrow$ Giảm tốc
2. Thường nhiều game là cho vận tốc tối đa luôn, nó khá dễ cho người mới nhưng lại bị khựng và tạo cảm giác như robot. Và về sau khi phát triển thêm có băng chuyền với lò xò bật nhảy có thể làm khó khăn hơn![[Pasted image 20260607152348.png]]
3. **Hãy sử dụng lực/forces**.: Cho phép di chuyển mượt mà và tận dụng cơ chế vật lý của unity nhưng nó lại khá khó điều khiển và thiếu phản hồi (?)![[Pasted image 20260607164635.png]]![[Pasted image 20260611061622.png]]
   Và trong hàm update đúng hơn là FixedUpdate thì video tính vận tốc mục tiêu (targetSpeed) bằng cách nhân moveInput với moveSpeed.
   Đồng thời từ đó mà tính được vận tốc lực cần thiết để thay đổi vận tốc hiện tại thành vận tốc mục tiêu: speedDif = targetSpeed - rb.linearvelocity.x. Tức là khi di chuyển theo hướng mong muốn ta sẽ áp dụng ít lực hơn nhiều![[Pasted image 20260611050808.png]]
   ÍT hơn so với việc cố gắng quay đầu nhân vật sang hướng khác. Nhưng nhờ code tính toán vậy lại giúp giảm bớt **sự vụng về khi sử dụng vật lý**! vì giúp nhân vật quay nhanh mang lại cảm giác mượt mà cho người chơi
4. **Sự khác biệt trong giá trị gia tốc và giảm tốc**: Là cách để kiểm soát tốc độ và để tăng lên một công suất nhất định để gia tốc phản hồi nhanh hơn và mượt hơn.
   Ban đầu là `accelRate` xác định trạng thái lấy giá trị tuyệt đối của targetspeed xem có lớn hơn 0.01f không, nếu lớn hơn thì tăng tốc ngược lại thì giảm tốc.
   Sau đó áp dụng gia tốc `movement` bằng cách nhân với 1 lũy thừa nhất định để tăng tốc lên tốc độ cao hơn tạo phản phản hồi nhanh và tạo cảm giác mượt mà hơn và nhân với dấu (dương hoặc âm) để áp dụng hướng.
   Cuối cùng là áp dụng lực đã tính toán bằng `rb.AddForce(movement * Vector2.right)` để chỉ áp dụng lên mỗi trục x, này ==không có làm nhảy== nên vậy
5. **Vấn đề khi người chơi muốn dừng lại**: khi muốn dừng thì cảm giác khá là trơn trượt, vấn đề càng mệt hơn khi để ma sát của người chơi về `0`. Và để khắc phục thì thêm **ma sát nhân tạo** khi người chơi cố dừng lại.![[Pasted image 20260611060722.png]]![[Pasted image 20260611061545.png]]
   Hoạt động bằng cách kiểm tra xem lượng ma sát có lớn hơn vận tốc hiện tại hay không rồi dùng giá trị nhỏ nhất để áp dụng một lực ngược lại với hướng chuyển động hiện tại như vậy giúp người chơi phanh nhanh hơn nhiều.
6. **GIờ là nhảy**: Cách đơn giản là kiểm tra xem người chơi có đang đứng trên mặt đất không và nhấn nút nhảy cùng lúc
   Có thể thực hiện nhanh bằng cách dùng **OverlapBox** để kiểm tra mặt đất![[Pasted image 20260611061940.png]]
   Sau đó dùng rigidbody.force thêm 1 lần nữa, nhưng lần này dùng force mode để nhảy. (là dùng Impulse, ForceMode2D.Impulse ấy)![[Pasted image 20260611062421.png]]
   ==Nhưng== nếu người chơi nhấn nhảy ngay lập tức khi chưa chạm đất hoàn toàn (vẫn trong trạng thái rơi nhưng nhìn sắp chạm hoặc trong hoảng khắc) hay khi vừa rơi khỏi nền đất trước khi nhấn nút nhảy thì sao? Với người chơi thì họ cảm giác như trò chơi chưa nhận đầu vào dù ta biết họ chưa có chạm đất. 
   Và để giải quyết vấn đề này là dùng **Coyote Time**, về cách triển khai thì tạo 2 bộ đếm thơi gia, 1 cho thời điểm người nhấn nút nhảy lần cuối, 2 cho thời điểm người chơi chạm đất lần cuối![[Pasted image 20260611063011.png]]
   Rồi sau đó kiểm tra trong mỗi khung hình xem cả 2 có đúng trong một khoảng thời gian ngắn hay không kiểu $\frac{1}{5}$ hoặc $\frac{1}{10}$ giây.![[Pasted image 20260611063651.png]]
   RỒi ta đặt lại bộ đếm này về giá trị đã chọn bất cứ khi nào người chơi nhảy hoặc chạm đất![[Pasted image 20260611063758.png]]![[Pasted image 20260611063824.png]]
   
   ==Còn một cách khác== nhưng mượt mà hơn là **Jump Cut**, đơn giản là cho phép người chơi kiểm soát cú nhảy bằng cách giữ phím nhảy để nhảy lâu hơn hoặc buông phím nhảy để hạ cánh sớm hơn, từ đó mang lại cảm giác điểu khiển có chiều hơn. 
   Để thêm tính năng này thì ta phải kiểm tra xem người chơi đã nhả phím nhảy hay chưa![[Pasted image 20260611064402.png]]
   Nếu đúng thì giảm vận tốc hiện tại của người chơi bằng một hệ số nhân, thường bằng 1 nửa giá trị khởi đầu là tốt.
   
   ==Thêm một thứ nữa== là **extra full gravity**, là có trọng lực cao hơn khi đã đạt đến phần đỉnh của cú nhảy thực sự giúp cú nhảy của người chơi nảy hơn và phản hồi nhanh hơn. Có thể thêm nhanh bằng cách điều chỉnh rigidbody scale khi đang rơi![[Pasted image 20260611065022.png]]
1. **Một vài mẹo khác**: 
   - ở đổi tượng người chơi (player gameobject), hãy dùng collider hình tròn giúp khắc phục sự cố người chơi bị kẹt trên mặt đất, đặc biệt là khi sử dụng tilemap.
   - Áp dụng vật liệu vật lý 2D (2D physic material để là 0 hết) cho player để không bị lỗi kẹt tường hoặc nền đất
   - Tăng trọng lực trong dự án, cái trọng lực phải tự cài và có trong tính toán tự động ấy, chỉnh trong setting của unity ấy. Chỉ chỉnh khi cảm giác game đang làm kiểu như bị lơ lửng quá, khuyến khích để là `-30`
   - Thiết lập rigidbody, để cả thiện khả năng phát hiện va chạm và vật lý của người chơi:
	   - Collision Detection = Continuous
	   - Sleeping Mode = Never Sleep
	   - Interporlate = Interpolate
# Video [Improve your Platformer with Acceleration | Examples in Unity](https://www.youtube.com/watch?v=KKGdDBFcu0Q)
1. **Bắt đầu với gia tốc/Acceleration**: 
   - 1 bước chạy của nhân vật được chia thành 3 giai đoạn: ==gia tốc/acceleration -> tốc độ tối đa/max speed -> giảm tốc/deceleration==, 3 thành phần cốt lõi này bảo vệ cảm giác cơ bản của nhân vật. Hầu hết nhà phát triển bắt đầu với platformer mà bỏ qua gia tốc, cách này tốt cho người mới bắt đầu nhưng đổi lại ta có 1 nhân vật khó điều khiển gần giống như robot nhưng ta có thể làm tốt hơn.
   - Trong unity, giải pháp thường nghĩ đến là dựa vào phương pháp dựa trên vật lý, nhưng việc dùng lực có thể tạo cảm giác rất khó xử và thiếu phản hồi bù lại sẽ có chuyển động mượt mà khi nhắm mục tiêu nhưng không dáng để đánh đổi khả năng điểu khiển.
     => Giải pháp là: ==Điều chỉnh lực tác dụng lên người chơ dựa trên tốc độ hiện tại của họ gần với tốc độ tối đa==
   - Nghĩa là nếu người chơi di chuyển nhanh, ta chỉ cần tác dụng 1 lực nhỏ, ngược lại nếu người chơi muốn quay đầu, ta sẽ tác dụng một lực lớn đảm bảo đổi hướng nhanh chóng. Vì thế mà chuyển luôn nhanh nhạy và phản hồi kịp thời cũng như đúng với quá trình giảm tốc mà không cần thêm bất kỳ ma sát nào.
   - Nhiều game platform không xoay quanh việc nhảy platform chính xác mà vài game như **Hollow Knight** nghiêng về gia tốc cao hơn. Thú vị là **Hollow Knight** cũng bỏ qua gia tốc (?? đọc phụ đề ghi thế) nhưng là có chủ ý vì nhà phát triển đơn giản hóa chuyển động nên cho người chơi cảm thấy được kiểm soát mọi thứ, và việc kiểm soát được như vậy lại quan trọng với 1 game có nhịp độ nhanh.
2. **Giảm gia tốc khi đang ở trên không**: Trong game **Celeste** vì việc giảm gia tốc khi nhân vật đang ở trên không đã tạo thêm sự mượt mà và tương phản cho chuyển động, Trong mã nguồn của game ta có thể thấy gia tốc cũng giảm sau khi bị đẩy khỏi bệ hoặc thực hiện một cú nhảy siêu cao, thú vị ở chỗ là nhờ vậy **Celeste** cho phép chơi với **động lượng/cơ chế lấy đà** ở cấp độ kỹ năng cao nhất trong khi vẫn duy trì điều khiển nhạy bén, yêu tố cốt lõi trong thiết kế trò chơi
   - Có thể bạn muốn người chơi cảm thấy **phản hồi nhanh hơn ở đỉnh cú nhảy**
3. **Thêm hệ số nhân khi rẽ/quay đầu**
4. Cơ chế **động lượng/lấy đà**, có thể coi là cơ khó thiết kế nhất vì chúng tạo cảm giác thiểu phản hồi nhưng khi làm chủ được thì nó lại cho ra cảm giác điểu khiển vô cùng thỏa mãn
# Video [Improve your Platformer’s Jump (and Wall Jump) | Unity](https://www.youtube.com/watch?v=2S3g8CgBG1g)
1. **Rơi nhanh/Faster Fall**: Tăng tốc độ khi rơi xuống, nói chung là việc để nhân vật có cảm giác lơ lửng trên không là không thực tế lắm nhưng này cũng tùy vì nhà phát triển có thể tạo kiểu rơi bồng bềnh hay gì đó nói chung là vậy
    - Nói dễ hiểu là khi đạt đỉnh cú nhảy thì tăng trọng lực ![[Pasted image 20260611102827.png]]và nó còn kết hợp tốt với **nhảy cao nhiêu/Jump height** để kiểm soát độ cao khi nhảy nữa. ![[Pasted image 20260611102853.png]]
2. **Nhảy cao nhiêu/Jump height**: Cho phép kiểm soát độ cao khi nhảy, kiểm soát theo kiểu người chơi nhảy dựa trên thời gian giữ nút nhảy. Có 2 cách làm: 1. **Velocity Cut/Tăng lục hút** và **Gravity Increase/Tăng trọng lực**:
    - Với **Velocity Cut/Tăng lục hút**: Game **Super Meat Boy** giảm **động lực/lấy đà** của hướng lên tầm 50% ngay khi người chơi thả nút nhảy. Tạo cảm giác cực kỳ chính xác và có thể hoạt động hoàn hảo cho game đang làm.
    - Với **Gravity Increase/Tăng trọng lực**: Tăng trọng lực của người chơi khi thả nút nhảy, cảm giác nhẹ nhàng hơn đồng thời tạo chuyền tiếp mượt mà trở lại trạng thái rơi, mặc dù có thể làm người chơi rơi khơi nhanh. Và nó cũng xảy ra với trọng lực đẩy người chơi xuống khi rơi.![[Pasted image 20260611104015.png]]![[Pasted image 20260611104029.png]]![[Pasted image 20260611104103.png]]
3. **Tốc độ tối đa/Max speed**: Trong nhiều trò chơi như **Hollow Knight**, giới hạn tốc độ tối đa của nhân vật được gọi là kẹp tốc độ, nhờ đó mà không khiến người rơi bị mất kiểm soát (như kiểu rơi xuống mà động cơ vẫn bật vậy)![[Pasted image 20260611104319.png]]
4. **Thời gian trên không/Air Time**: Đây là phần quan trọng vì nếu không làm tốt có thể gây ra khó chịu và cũng là phần có thể chỉnh xem nhảy được bao xa (**độ dài cú nhảy**)![[Pasted image 20260611104615.png]]![[Pasted image 20260611104625.png]]
    Game **Hollow Knight** có rất nhiều thời gian trên không khi nhảy và nhờ đó mà cho phép người dễ dàng căn chỉnh các đòn tấn công, Mặt khác **Celeste** lại có cú nhảy ngắn, tạo cảm giác nhảy nhanh và nảy từ đó làm người chơi phải tính toán ở mỗi cú nhảy
5. **Thời gian lơ lửng/Jump Hang**: Dễ hiểu thì là khoảng thời gian mà nhân vật dường như có cảm giác "lơ lửng/trôi" hoặc giảm tốc độ rơi khi đạt đến đỉnh điểm cú nhảy,![[Pasted image 20260611105804.png]]Theo đồ thị thì phần màu trắng là chỗ đạt điểm cao nhất nên sẽ giảm trọng lực (giảm tầm 1 nửa) ở đó 
	![[Pasted image 20260611105822.png]] giúp cú nhảy bớt cứng nhắc và tự nhiên hơn. Người chơi có 1 khoảng khác để căn chỉnh vị trí rơi xuống hoặc đưa ra quyết định tiếp theo![[Pasted image 20260611105836.png]]
6. **Bonus Peak Speed/Tăng cường gia tốc ngang**: Dễ hiểu là khi người chơi đạt đỉnh cú nhảy thì vận tốc thẳng đứng sẽ gần bằng 0 và trong trạng thái lơ lửng thì độ gia tốc theo chiều ngang x sẽ được gia cường để người chơi có thể điểu khiển vị trí rơi xuống. Tức là cho người chơi dễ điều khiển hơn.![[Pasted image 20260611110537.png]]
7.  **Thị giác/Visual**: Cải hiện hình ảnh, làm rõ trạng thái của nhân vật như nhảy, bám tường, ăn đòn. Nhưng khi thảo luận về chuyển động thì hiệu ứng **co và giãn (motion squash and stretch)**![[Pasted image 20260611111010.png]]
    Có lẽ là quan trong nhất và cả các **hạt va chạm (impact particles)**. Hoạt họa kết hợp chuyển động được thiết kế chu đáo có thể biến một bộ điểu khiển đơn giản thành một thứ rất tuyệt vời.
8. **Lực nhảy/Jump Force**: Là yếu tồ quyết định độ cao và cảm giác cú nhảy.
    Mà cũng nhiều cách làm: Như là bỏ qua **vận tốc hiện tại/override velocity** (đúng hơn là ghi đè) ![[Pasted image 20260611111613.png]]
    Lý do dùng cách này vì khi đang rơi với tốc độ cao, vận tốc âm sẽ rất lớn nếu chỉ cộng thêm 1 lực nhảy cố định thì cú nhảy sẽ bị yếu hoặc không đạt được độ cao mong muốn vì nó dango9 chiến đầu với vận tốc rơi đang có. nên khi ghi đè thì dù có đang đứng yên hay đang rơi, khi nhấn nhảy vận tốc thẳng đứng sẽ ngay lập tức được thiết lập thành giá trị dương cố định từ đó tạo ra sự nhất quán
    Về **phường pháp cộng dồn/Factor in Momentum**: Sẽ theo vật lý, cảm giác nhảy nhạy bén nhất, nhưng làm mệt nhất khi phải làm sao để cho cú nhảy được cộng dồn thắng được để cho ra độ cao đúng với kỳ vọng mà không bị vấn đề nhảy liên tiếp mà cú nhảy sau thấp hơn cú nhảy trước, kiểu nếu dùng phương pháp này vào nhảy đôi thì nhảy lần đầu đúng kỳ vọng còn sang lần nhảy thứ 2 đang trên không thì cảm giác thấp hơn chẳng hạn và nếu đang ở vận tốc di chuyển ngang cao mà nhấn nhảy là sẽ bay được rất xa nhưng cái kiểu này lại tạo ra cảm giác động lực/lấy đà![[Pasted image 20260611112837.png]]![[Pasted image 20260611112852.png]]
9. **Thời gian châm chước/Grace Timers**: ĐÚng hơn là **Coyote Time** đơn giản là cho người chơi thêm chút xíu thời gian khi đã ra khỏi mép nền, vì người chơi không thể thao tác chuẩn xác tuyệt đối nên rất dễ nhấn hụt, nên việc tạo ra độ trễ là để cho công bằng và chiều người chơi hơn
    - **Input Buffer/Thời gian chờ lệnh nhảy**: là cơ chế cho pháp hệ thống ==ghi nhớ== lệnh nhảy của người chơi trong vài mili giây ngay cả khi nhân vật chưa chạm đất (ví dụ: nhấn nhảy ngay trước khi vừa chạm sàn), cần thiết để tránh tình trạng nhân vật không phải hồi lệnh khi người chơi nhấn nút nhảy xớm hơn một chút so với khoảng khắc tiếp đất hoàn hảo.
    - Tác giả có khuyên là môi game platformer đều nên có **Coyote Time** để giảm bớt sự quan trọng của việc nhập liệu phải chính xác mà tập trung vào làm chủ kỹ năng di chuyển hơn, giúp game bớt cảm giác "thiếu phản hồi".  
10. **Nhảy tường/Wall Jump**: là cơ chế cho phép nhân vật bật ra khỏi bề mặt thảng dứng. Là thử thách về mặt lập trình vì nó kết hợp cả lực nhảy theo hướng thẳng đứng (Y) và hướng ngang (X) nói chung là vẫn tuân thủ các quy tắc nhảy cơ bản như đã đề cập bên trên nhưng vì thêm lực ngang nên sẽ khó làm hơn.
	-  Vấn đề chính là lực đẩy ngang của cú nhảy thường bị xung đột với lực di chuyển của nhân vật, khiến cú nhảy bị triệt tiêu ngay lập tức
	- Cách làm của tác giả là ngăn chặn luôn chuyển động của người chơi trong 1 khoảng thời gian ngắn khi nhảy tường![[Pasted image 20260611152712.png]]
	  Được nhưng theo tác giả thì khá vụng về và còn cách khác nữa
	- Cách thứ 2 là giảm tốc độ di chuyển hiện tại, không loại bỏ lực chạy. Như vậy vẫn giữ được kiểm soát và cũng chuyển đổi mượt mà hơn. Đồng thời là cho ta quyết định mực độ khó dễ của trò chơi khi có thể điều chỉnh xem người chơi có thể quay lại tường sau khi bật ra không. Và nên dùng **LERP (Linear Interpolation)** để làm mượt chuyển động bằng tham số thời gian để làm giảm độ giật.
11. **Trượt tường/Slide**: Là một kĩ thuật nâng cao, và theo như tui hiểu thì phần này cũng tương tự với hệ thống chạy/run thôi và vì là kĩ thuật nâng cao nên bị cắt giảm khỏi video**Code**![[Pasted image 20260611153442.png]]
12. **Thiết kế nhảy/Jump Design**: 
    - Trong **Hollow Knight**, game cho phép nhảy tường nhanh rồi sau đó trả lại quyền kiểm soát cho người chơi khi rời khỏi tường![[Pasted image 20260611154141.png]]
      Thời gian nhảy ngắn với giá trị LERP cao, cho phép người chơi dễ dàng quay lại tường sau khi đã nhảy khỏi tường từ đó cho phép người chơi leo lên trên. Vì đẩy là game _Metroidvania_ thiên về hành động nên nhảy chỉ là bổ sung mà thôi.
    - Ở **Celeste** thì mang cảm giác mất quyền kiểm soát hơn khi không cho người chơi có thể quay lại tường để leo lên nữa mà còn kết hợp thêm cả cơ chế thể lực để tăng giới hạn nhưng thú vị là có **lướt/dash**. Game cho phép kết hợp cú nhảy với **lướt/dash** để di chuyển nên dù bị giới hạn nhưng game lại cho cảm giác đã tay hơn khi di chuyển. Nói thẳng ra game coi leo trèo nhảy là một nguồn tài nguyên nên người chơi phải tính toán để thực hiện thành động.
13. **Nhảy đổi/double Jump**: Là cơ chế thiết yếu trong các game platformer rồi. Để hoạt động thì phải cho người chơi nhảy ít nhất 1 lần khi đang ở trên không nếu chưa chạm đất ![[Pasted image 20260611155140.png]]
    và khi chạm đất trở lại thì ta đặt lại cái biến thôi![[Pasted image 20260611155343.png]]
    Mà cách game **Hollow Knight** dùng nhảy đôi khá hay là kết hợp với cả thủ thuật Pogo (thao tác tấn công xuống dưới để tạo lực đẩy lên) với thủ thuật này có thể thêm vào game để tạo thành chuỗi combo nhảy đôi -lướt cho phép duy trì trên không trong thời gian dài. Mà cái này bảo cũng tương tự ở trò **Shovel Knight**
14. **Cộng hưởng kĩ năng/Ability Synergy**: Dễ hiểu là kết hợp từ nhiều chức năng khác nhau như di chuyển, nhảy đôi, pogo, dash, v.v từ đó tạo ra các tổ hợp khác nhau. Các chức năng tưởng chừng như riêng lẻ lại có thể được code để bổ trợ lẫn nhau. 
    - ==Không làm tăng độ phức tập==: Chỉ là các chức năng cơ bản thôi, chỉ vài nút cho người chơi dễ nhớ nhưng lại có thể tạo ra hàng chục, hàng tỉ, rất nhiều combo khác nhau, đó là một cơ chế di chuyển hay như **Celeste** vậy
    
    

# Video: https://www.youtube.com/watch?v=Bsy8pknHc0M Này là của godot nhưng cứ xem đi
1. Dùng mẫu **trạng thái** làm cơ bản: 
   BaseState $\Rightarrow$ MoveState $\Rightarrow$ InputMoveState $\Rightarrow$ WalkState
							  $\Rightarrow$ RanState
							  $\Rightarrow$ DasState
2. **Đừng dùng rigidbody cho di chuyển**: Game platformer không hay thực tế mà lại dùng rigibody cho việc di chuyển thì không cần thiết lắm do rigidody sinh ra để mô phòng vật lý thực tế $\Rightarrow$ dùng **Kinematic** là hợp lý và tối ưu rồi.
3. **Vấn đề khi nhảy**: Người chơi đôi lúc sẽ nhấn nút nhảy sớm hơn vài frame trong khi nhân vật đang ở trên không từ đó khi nhân vật tiếp đất là đứng im luôn, vậy nên làm một bộ nhớ/bộ đệm ghi nhớ việc người chơi nhấn nút nhảy và tự động nhảy trong thời gian quy định nếu nhân vật tiếp đất trong khoảng thời gian đó. Từ đó thu hẹp dự định của người chơi và độ chính xác từng mili giây và cũng tạo cảm giác điều khiển mượt mà hơn. Làm thì đơn giản là lắng nghe lệnh nhảy hay khi người chơi nhấn phím nhảy rồi thêm bộ đếm thời gian là oke, nếu người chơi tiếp đất trước khi hết thời gian thì tự động  nhảy còn không thì đứng im.
4. **Thêm Coyote**: Dễ hiểu thì đây là tên của một nhân vật cartoon nào đó khi mà nhân vật đi ra mép đá thì một lúc sau mới rơi:)) Việc thêm Coyote vào là để người chơi khi bước ra vách thì vẫn có thể nhấn được nút nhảy, kiểu cho châm chước ấy thường nhiều game nổi tiếng làm vậy. Về lý do thì thường người chơi nhấn di chuyển ra chỗ vách luôn kiểu đi lố một quãng xong mới bấm nhảy dẫn đến việc cái check mặt đất đã rời hỏi mặt đất rồi nên việc thêm Coyote vào là cũng hợp lý. Việc thêm vào thì cũng dễ, khi người chơi vào chuyển trạng thái từ đang đứng trên mặt đất sang trạng thái rơi thì cho biến đếm thời gian và biến theo dõi tín hiệu nhảy vào là xong, về thời gian cho phép nhảy sau khi rời hỏi mặt đất là bao nhiêu thì tự thiết lập thôi. **Lưu ý**: Phải là từ trạng thái *đang trên mặt đất* sang trạng thái *rơi* nha không kẻo lại thành từ *đang trên mặt đất* sang *nhảy*  thì lại nhảy thêm phát thành *nhảy đôi* trong khi game không cho nhảy đôi thì chết.
   $\Rightarrow$ **Nói chung là không ai có thể chính xác tuyệt đối nên cứ cho sai số một xíu đi**
   ==Bổ sung==: Nếu game có double jump, Coyote Time cũng giúp người chơi không bị mất lần nhảy đôi oan khi chỉ bị trễ vài frame.
5. **Đẩy người chơi ra khỏi gờ**: Này là chống người chơi cụng đầu ở mấy màn thấp à?
   Đúng luôn và làm vậy vì nếu có một map mà cao đúng bằng nhân vật người chơi đang điều khiển mà bên phải lại có phần cao hơn thì khi nhảy cho tự động đẩy nhân vật sang chỗ cao hơn đó cũng như là cho phép thực hiện cú nhảy cao tối đa luôn.
   Về cách làm thì dùng **raycast** trên đầu người chơi, theo video là 4 tia và kiểm tra xem khi người chơi nhảy có tia nào chạm vào tường, trần, mặt đất không nếu tia ngoài hit nhưng tia trong + tia đối diện không hit thì đẩy người chơi ra khỏi phía tia bị chạm. Này chỉ thấy chỉ mẹo chứ chưa đi sâu vào.
6. **Tự viết hàm nhảy tùy chỉnh**: Theo video thì tùy những game có thể game vượt chướng ngại vật, bình thường, v.v mà hãy code hàm nhảy theo nhiều cách khác nhau, như là dùng fast falling hay nhảy kiểu parabol hya kiểu hình sin v.v hoặc xem các video làm thuật toán về game như: [**Math for Game Programmers: Building a Better Jump**](https://www.youtube.com/watch?v=hG9SzQxaCm8) 
   Có thể tăng trọng lực trong trạng thái rơi, giữ nút nhảy để nhảy cao hơn và buông nút nhảy để rơi sớm hơn kèm nhảy thấp hơn chẳng hạn hoặc thiết kế kiểu có thể nhảy cao được bao nhiêu và thời gian đạt được chiều cao đó
   ==Bổ sung==: Khi buông nút nhảy sớm: có thể zero out velocity ngay, hoặc nhân velocity với một hệ số (ví dụ 0.5) để rơi mượt hơn thay vì dừng đột ngột.
