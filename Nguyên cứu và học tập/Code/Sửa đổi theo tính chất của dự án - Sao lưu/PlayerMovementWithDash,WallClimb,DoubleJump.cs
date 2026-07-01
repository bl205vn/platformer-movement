/*
	Tạo bởi @DawnosaurDev tại youtube.com/c/DawnosaurStudios
	Cảm ơn bạn rất nhiều vì đã xem qua và tôi hy vọng bạn thấy nó hữu ích! 
	Nếu bạn có bất kỳ thắc mắc, câu hỏi hoặc phản hồi nào, vui lòng liên hệ trên twitter của tôi hoặc để lại bình luận trên youtube :D

	Hãy thoải mái sử dụng nó trong các trò chơi của riêng bạn và tôi rất muốn xem bất cứ thứ gì bạn làm ra!
 */

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//Scriptable object chứa tất cả các thông số di chuyển của người chơi. Nếu bạn không muốn sử dụng nó
	//chỉ cần dán tất cả các thông số vào, mặc dù bạn sẽ cần thay đổi thủ công tất cả các tham chiếu trong script này
	public PlayerDataWithDash Data;

	#region COMPONENTS
    public Rigidbody2D RB { get; private set; }
	#endregion

	#region STATE PARAMETERS
	//Các biến kiểm soát các hành động khác nhau mà người chơi có thể thực hiện bất cứ lúc nào.
	//Đây là các trường public cho phép các script khác đọc chúng
	//nhưng chỉ có thể ghi vào một cách private (nội bộ).
	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }
	public bool IsWallJumping { get; private set; }
	public bool IsWallJumpLocked => IsWallJumping && Time.time - _wallJumpStartTime < Data.wallJumpTime;
	public bool IsDashing { get; private set; }
	public bool IsSliding { get; private set; }

	//Bộ đếm thời gian (cũng là các trường, có thể là private và sử dụng một phương thức trả về bool)
	public float LastOnGroundTime { get; private set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }

	//Nhảy
	private bool _isJumpCut;
	private bool _isJumpFalling;

	//Nhảy tường
	private float _wallJumpStartTime;
	private int _lastWallJumpDir;

	//Lướt
	private int _dashesLeft;
	private bool _dashRefilling;
	private Vector2 _lastDashDir;
	private bool _isDashAttacking;

	//Nhảy đôi
	private int _bonusJumpsLeft;

	#endregion

	#region INPUT PARAMETERS
	private Vector2 _moveInput;

	public float LastPressedJumpTime { get; private set; }
	public float LastPressedDashTime { get; private set; }
	#endregion

	#region CHECK PARAMETERS
	//Thiết lập tất cả những thứ này trong inspector
	[Header("Checks")] 
	[SerializeField] private Transform _groundCheckPoint;
	//Kích thước của groundCheck phụ thuộc vào kích thước nhân vật của bạn, thông thường bạn muốn chúng nhỏ hơn một chút so với chiều rộng (đối với mặt đất) và chiều cao (đối với kiểm tra tường)
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	#endregion

    private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}

	private void Update()
	{
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		LastPressedDashTime -= Time.deltaTime;
		#endregion

		#region INPUT HANDLER
		_moveInput.x = Input.GetAxisRaw("Horizontal");
		_moveInput.y = Input.GetAxisRaw("Vertical");

		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        {
			OnJumpInput();
        }

		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
		{
			OnJumpUpInput();
		}

		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K))
		{
			OnDashInput();
		}
		#endregion

		#region COLLISION CHECKS
		if (!IsDashing && !IsJumping)
		{
			//Kiểm tra mặt đất
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //kiểm tra xem hộp đã thiết lập có chồng lấp với mặt đất không
			{
				LastOnGroundTime = Data.coyoteTime; //nếu có, đặt lastGrounded thành coyoteTime
            }		

			//Kiểm tra tường phải
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
					|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
				LastOnWallRightTime = Data.coyoteTime;

			//Kiểm tra tường trái
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
				|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
				LastOnWallLeftTime = Data.coyoteTime;

			//Cần hai kiểm tra cho cả tường trái và phải vì bất cứ khi nào người chơi quay lại, các checkPoint của tường sẽ đổi bên
			LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
		}
		#endregion

		#region JUMP CHECKS
		if (IsJumping && RB.velocity.y < 0)
		{
			IsJumping = false;

			if(!IsWallJumping)
				_isJumpFalling = true;
		}

		if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
		{
			IsWallJumping = false;
		}

		if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
			_isJumpCut = false;

			if(!IsJumping)
				_isJumpFalling = false;

			_bonusJumpsLeft = Data.bonusJumpAmount; // reset khi chạm đất
		}

		if (!IsDashing)
		{
			//Nhảy
			if (CanJump() && LastPressedJumpTime > 0)
			{
				IsJumping = true;
				IsWallJumping = false;
				_isJumpCut = false;
				_isJumpFalling = false;
				Jump();
			}
			//NHẢY TƯỜNG
			else if (CanWallJump() && LastPressedJumpTime > 0)
			{
				IsWallJumping = true;
				IsJumping = false;
				_isJumpCut = false;
				_isJumpFalling = false;

				_wallJumpStartTime = Time.time;
				_lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

				WallJump(_lastWallJumpDir);
			}
			//NHẢY ĐÔI
			else if (CanDoubleJump() && LastPressedJumpTime > 0)
			{
				IsJumping = true;
				IsWallJumping = false;
				_isJumpCut = false;
				_isJumpFalling = false;
				_bonusJumpsLeft--;
				Jump();
			}
		}
		#endregion

		#region DASH CHECKS
		if (CanDash() && LastPressedDashTime > 0)
		{
			//Đóng băng trò chơi trong một tích tắc. Thêm độ "juicy" (mượt mà/thú vị) và châm chước một chút cho phím bấm hướng
			Sleep(Data.dashSleepTime); 

			//Nếu không nhấn hướng nào, lướt tới phía trước
			if (_moveInput != Vector2.zero)
				_lastDashDir = _moveInput;
			else
				_lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;



			IsDashing = true;
			IsJumping = false;
			IsWallJumping = false;
			_isJumpCut = false;

			StartCoroutine(nameof(StartDash), _lastDashDir);
		}
		#endregion

		#region SLIDE CHECKS
		if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
			IsSliding = true;
		else
			IsSliding = false;
		#endregion

		#region GRAVITY
		if (!_isDashAttacking)
		{
			//Trọng lực cao hơn nếu chúng ta nhả nút nhảy hoặc đang rơi
			if (IsSliding)
			{
				SetGravityScale(0);
			}
			else if (RB.velocity.y < 0 && _moveInput.y < 0)
			{
				//Trọng lực cao hơn nhiều nếu giữ phím mũi tên xuống
				SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
				//Giới hạn tốc độ rơi tối đa, để khi rơi từ khoảng cách xa, chúng ta không tăng tốc đến mức quá nhanh
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
			}
			else if (_isJumpCut)
			{
				//Trọng lực cao hơn nếu nhả nút nhảy
				SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
			}
			else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
			{
				SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
			}
			else if (RB.velocity.y < 0)
			{
				//Trọng lực cao hơn nếu đang rơi
				SetGravityScale(Data.gravityScale * Data.fallGravityMult);
				//Giới hạn tốc độ rơi tối đa, để khi rơi từ khoảng cách xa, chúng ta không tăng tốc đến mức quá nhanh
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
			}
			else
			{
				//Trọng lực mặc định nếu đứng trên nền hoặc di chuyển lên trên
				SetGravityScale(Data.gravityScale);
			}
		}
		else
		{
			//Không có trọng lực khi đang lướt (trở lại bình thường khi giai đoạn dashAttack ban đầu kết thúc)
			SetGravityScale(0);
		}
		#endregion
    }

    private void FixedUpdate()
	{
		//Xử lý Chạy
		if (!IsDashing)
		{
			if (IsWallJumping)
				Run(Data.wallJumpRunLerp);
			else
				Run(1);
		}
		else if (_isDashAttacking)
		{
			Run(Data.dashEndRunLerp);
		}

		//Xử lý Trượt
		if (IsSliding)
		{
			if (_moveInput.y > 0)
				WallClimb();
			else
				Slide();
		}
    }

    #region INPUT CALLBACKS
	//Các phương thức xử lý đầu vào được phát hiện trong Update()
    public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut() || CanWallJumpCut())
			_isJumpCut = true;
	}

	public void OnDashInput()
	{
		LastPressedDashTime = Data.dashInputBufferTime;
	}
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}

	private void Sleep(float duration)
    {
		//Phương thức được sử dụng để chúng ta không cần gọi StartCoroutine ở khắp mọi nơi
		//Cú pháp nameof() có nghĩa là chúng ta không cần nhập trực tiếp một chuỗi.
		//Loại bỏ khả năng sai lỗi chính tả và sẽ cải thiện thông báo lỗi nếu có
		StartCoroutine(nameof(PerformSleep), duration);
    }

	private IEnumerator PerformSleep(float duration)
    {
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(duration); //Phải là Realtime vì timeScale sẽ bằng 0 
		Time.timeScale = 1;
	}
    #endregion

	//CÁC PHƯƠNG THỨC DI CHUYỂN
    #region RUN METHODS
    private void Run(float lerpAmount)
	{
		//Tính toán hướng chúng ta muốn di chuyển và vận tốc mong muốn
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		//Chúng ta có thể giảm bớt sự điều khiển bằng Lerp() điều này làm mượt các thay đổi về hướng và tốc độ
		targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Lấy giá trị gia tốc dựa trên việc chúng ta đang tăng tốc (bao gồm cả việc quay đầu) 
		//hoặc đang cố gắng giảm tốc (dừng lại). Cũng như áp dụng hệ số nhân nếu chúng ta đang ở trên không.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Tăng tốc độ và tốc độ tối đa của chúng ta khi ở đỉnh điểm của cú nhảy, làm cho cú nhảy có cảm giác nảy hơn, phản hồi tốt hơn và tự nhiên hơn
		if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//Chúng ta sẽ không làm chậm người chơi nếu họ đang di chuyển theo hướng mong muốn nhưng ở tốc độ lớn hơn tốc độ tối đa của họ
		if(Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Ngăn chặn bất kỳ sự giảm tốc nào xảy ra, hoặc nói cách khác là bảo toàn động lượng hiện tại
			//Bạn có thể thử nghiệm cho phép người chơi tăng nhẹ tốc độ của họ trong "trạng thái" này
			accelRate = 0; 
		}
		#endregion

		//Tính toán sự khác biệt giữa vận tốc hiện tại và vận tốc mong muốn
		float speedDif = targetSpeed - RB.velocity.x;
		//Tính toán lực dọc theo trục x để áp dụng cho người chơi

		float movement = speedDif * accelRate;

		//Chuyển đổi điều này thành một vector và áp dụng cho rigidbody
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

		/*
		 * Dành cho những ai quan tâm, đây là những gì AddForce() sẽ làm
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime mặc định trong Unity là 0.02 giây tương đương với 50 lần gọi FixedUpdate() mỗi giây
		*/
	}

	private void Turn()
	{
		//lưu tỷ lệ và lật người chơi dọc theo trục x, 
		Vector3 scale = transform.localScale; 
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}
    #endregion

    #region JUMP METHODS
    private void Jump()
	{
		//Đảm bảo chúng ta không thể gọi Nhảy nhiều lần từ một lần nhấn
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		//Chúng ta tăng lực áp dụng nếu chúng ta đang rơi
		//Điều này có nghĩa là chúng ta sẽ luôn cảm thấy như nhảy cùng một độ cao 
		//(đặt vận tốc Y của người chơi về 0 trước đó có thể sẽ hoạt động tương tự, nhưng tôi thấy cách này thanh lịch hơn :D)
		float force = Data.jumpForce;
		if (RB.velocity.y < 0)
			force -= RB.velocity.y;

		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion
	}

	private void WallJump(int dir)
	{
		//Đảm bảo chúng ta không thể gọi Nhảy tường nhiều lần từ một lần nhấn
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		LastOnWallRightTime = 0;
		LastOnWallLeftTime = 0;

		#region Perform Wall Jump
		Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
		force.x *= dir; //áp dụng lực theo hướng ngược lại của tường

		if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
			force.x -= RB.velocity.x;

		if (RB.velocity.y < 0) //kiểm tra xem người chơi có đang rơi hay không, nếu có chúng ta trừ đi vận tốc y (chống lại lực hấp dẫn). Điều này đảm bảo người chơi luôn đạt được lực nhảy mong muốn của chúng ta hoặc lớn hơn
			force.y -= RB.velocity.y;

		//Không giống như khi chạy, chúng ta muốn sử dụng chế độ Impulse.
		//Chế độ mặc định sẽ áp dụng lực ngay lập tức bỏ qua khối lượng
		RB.AddForce(force, ForceMode2D.Impulse);
		#endregion

		#region Turn On Wall Jump
		//Xoay mặt nhân vật theo hướng nhảy tường nếu cờ doTurnOnWallJump được bật
		if (Data.doTurnOnWallJump)
		{
			CheckDirectionToFace(dir > 0);
		}
		#endregion
	}
	#endregion

	#region DASH METHODS
	//Coroutine Lướt
	private IEnumerator StartDash(Vector2 dir)
	{
		//Nhìn chung phương pháp lướt này nhằm mục đích bắt chước Celeste, nếu bạn đang tìm kiếm
		// một phương pháp dựa trên vật lý hơn hãy thử một phương pháp tương tự như phương pháp được sử dụng trong lúc nhảy

		LastOnGroundTime = 0;
		LastPressedDashTime = 0;

		float startTime = Time.time;

		_dashesLeft--;
		_isDashAttacking = true;

		SetGravityScale(0);

		//Chúng ta giữ vận tốc của người chơi ở tốc độ lướt trong giai đoạn "tấn công" (trong celeste là 0.15s đầu tiên)
		while (Time.time - startTime <= Data.dashAttackTime)
		{
			RB.velocity = dir.normalized * Data.dashSpeed;
			//Tạm dừng vòng lặp cho đến khung hình tiếp theo, tạo ra một thứ gì đó giống như vòng lặp Update. 
			//Đây là một cách triển khai sạch sẽ hơn so với việc sử dụng nhiều bộ đếm thời gian và cách tiếp cận coroutine này thực sự là những gì được sử dụng trong Celeste :D
			yield return null;
		}

		startTime = Time.time;

		_isDashAttacking = false;

		//Bắt đầu "kết thúc" của quá trình lướt nơi chúng ta trả lại một số quyền kiểm soát cho người chơi nhưng vẫn giới hạn gia tốc chạy (xem Update() và Run())
		SetGravityScale(Data.gravityScale);
		RB.velocity = Data.dashEndSpeed * dir.normalized;

		while (Time.time - startTime <= Data.dashEndTime)
		{
			yield return null;
		}

		//Lướt xong
		IsDashing = false;
	}

	//Khoảng thời gian ngắn trước khi người chơi có thể lướt lại
	private IEnumerator RefillDash(int amount)
	{
		//Thời gian hồi chiêu ngắn, vì vậy chúng ta không thể liên tục lướt trên mặt đất, một lần nữa đây là cách triển khai trong Celeste, hãy thoải mái thay đổi nó
		_dashRefilling = true;
		yield return new WaitForSeconds(Data.dashRefillTime);
		_dashRefilling = false;
		_dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
	}
	#endregion

	#region OTHER MOVEMENT METHODS
	private void Slide()
	{
		//Hoạt động giống như Chạy (Run) nhưng chỉ theo trục y
		//Điều này có vẻ hoạt động tốt, nhưng có thể bạn sẽ tìm thấy một cách tốt hơn để triển khai trượt vào hệ thống này
		float speedDif = Data.slideSpeed - RB.velocity.y;	
		float movement = speedDif * Data.slideAccel;
		//Vì vậy, chúng ta giới hạn di chuyển ở đây để ngăn chặn bất kỳ sự điều chỉnh quá mức nào (những điều này không đáng chú ý trong lúc Chạy)
		//Lực được áp dụng không thể lớn hơn sự khác biệt tốc độ (âm) * theo số lần FixedUpdate() được gọi một giây. Để biết thêm thông tin, hãy nghiên cứu cách lực được áp dụng cho rigidbodies.
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		RB.AddForce(movement * Vector2.up);
	}

	private void WallClimb()
	{
		// Giống Slide() nhưng đẩy lên trên thay vì kéo xuống
		float speedDif = Data.wallClimbSpeed - RB.velocity.y;
		float movement = speedDif * Data.wallClimbAccel;
		movement = Mathf.Clamp(
			movement,
			-Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime),
			Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime)
		);
		RB.AddForce(movement * Vector2.up);
	}
    #endregion


    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

    private bool CanJump()
    {
		return LastOnGroundTime > 0 && !IsJumping;
    }

	private bool CanDoubleJump()
	{
		return _bonusJumpsLeft > 0 && LastOnGroundTime <= 0 && !IsWallJumping;
	}

	private bool CanWallJump()
    {
		return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
			 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
	}

	private bool CanJumpCut()
    {
		return IsJumping && RB.velocity.y > 0;
    }

	private bool CanWallJumpCut()
	{
		return IsWallJumping && RB.velocity.y > 0;
	}

	private bool CanDash()
	{
		if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
		{
			StartCoroutine(nameof(RefillDash), 1);
		}

		return _dashesLeft > 0;
	}

	public bool CanSlide()
    {
		if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
			return true;
		else
			return false;
	}
    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
	}
    #endregion
}

// tạo bởi Dawnosaur :D