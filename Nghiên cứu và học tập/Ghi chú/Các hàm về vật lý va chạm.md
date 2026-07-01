1. ==**Physics2D.OverlapBox**==: dùng để **kiểm tra xem có Collider nào đang nằm trong vùng hình chữ nhật** mà bạn tự định nghĩa hay không. Dùng thay cho `OnCollisionEnter2D` vì `OnCollisionEnter2D` dễ gây lỗi nhảy vô hạn và các bug khó chịu khác. `Physics2D.OverlapBox` chỉ đơn giản vẽ một **khung hình chữ nhật mỏng** để kiểm tra va chạm, không phức tạp như `OnCollisionEnter2D`.

==Cú pháp và giải thích tham số== **Cú pháp:** `Physics2D.OverlapBox(Vector2 point, Vector2 size, float angle, LayerMask layerMask)`

|Tham số|Kiểu dữ liệu|Ý nghĩa|
|---|---|---|
|`point`|`Vector2`|Tọa độ **tâm** của hình chữ nhật kiểm tra|
|`size`|`Vector2`|**Kích thước** của hình chữ nhật (chiều rộng, chiều cao)|
|`angle`|`float`|**Góc xoay** của hình chữ nhật (thường để `0f` nếu không cần xoay)|
|`layerMask`|`LayerMask`|**Layer** cần kiểm tra (ví dụ: layer "Ground")|

**Kiểu dữ liệu trả về:** `Collider2D` — trả về Collider đầu tiên tìm thấy trong vùng đó, hoặc `null` nếu không có gì.

==Ví dụ minh họa==

```C#
// Khai báo các biến cần thiết
public Transform groundCheck;       // Vị trí tâm của khung kiểm tra (gắn vào chân nhân vật)
public Vector2 groundCheckSize;     // Kích thước khung kiểm tra, ví dụ: (0.9f, 0.1f)
public LayerMask groundLayer;       // Layer "Ground" chọn trong Inspector

// Dùng trong hàm Update() hoặc FixedUpdate()
bool isGrounded = Physics2D.OverlapBox(
    groundCheck.position,   // Tâm hình chữ nhật
    groundCheckSize,        // Kích thước hình chữ nhật
    0f,                     // Góc xoay (0 = không xoay)
    groundLayer             // Chỉ kiểm tra với Layer "Ground"
) != null;                  // Nếu có Collider → đang đứng trên mặt đất
```

==Ứng dụng thực tế trong làm game== `Physics2D.OverlapBox` thường được dùng để kiểm tra trạng thái nhân vật một cách chính xác và ổn định:

- **Kiểm tra đang đứng trên mặt đất (isGrounded):** Đặt một điểm kiểm tra ở chân nhân vật, vẽ khung mỏng ngang. Nếu khung đó chạm Layer "Ground" → cho phép nhảy.
- **Kiểm tra đang chạm trần nhà:** Tương tự nhưng đặt điểm kiểm tra ở đỉnh đầu nhân vật, dùng để cắt lực nhảy khi bị chặn phía trên.
- **Khu vực kích hoạt sự kiện:** Kiểm tra xem kẻ địch hoặc vật phẩm có nằm trong vùng tấn công của nhân vật không.

==Lưu ý quan trọng: Dùng Gizmo để vẽ khung trực quan== Vì khung kiểm tra này **vô hình trong Scene**, bạn bắt buộc phải dùng `OnDrawGizmos` để vẽ nó ra cho dễ chỉnh trong Editor:

```C#
void OnDrawGizmos()
{
    Gizmos.color = Color.red; // Chọn màu khung (đỏ cho dễ thấy)
    Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
}
```

==So sánh với OnCollisionEnter2D==

|                 | Physics2D.OverlapBox                                | OnCollisionEnter2D               |
| --------------- | --------------------------------------------------- | -------------------------------- |
| Cách hoạt động  | Kiểm tra chủ động mỗi frame                         | Chờ sự kiện va chạm xảy ra       |
| Lỗi nhảy vô hạn | ✅ Không bị                                          | ❌ Dễ bị                          |
| Kiểm soát       | Cao (tự chọn vị trí, kích thước, layer)             | Thấp hơn                         |
| Độ phức tạp     | Đơn giản                                            | Phức tạp hơn                     |
| Khi nào dùng    | Kiểm tra trạng thái (isGrounded, isTouchingWall...) | Xử lý va chạm có vật lý phức tạp |

2. ==**Physics2D.OverlapCircle**==: dùng để **kiểm tra xem có Collider nào đang nằm trong vùng hình tròn** mà bạn tự định nghĩa hay không. Về bản chất hoạt động giống hệt `OverlapBox`, chỉ khác ở chỗ vùng kiểm tra là **hình tròn** thay vì hình chữ nhật.

==Cú pháp và giải thích tham số== **Cú pháp:** `Physics2D.OverlapCircle(Vector2 point, float radius, LayerMask layerMask)`

|Tham số|Kiểu dữ liệu|Ý nghĩa|
|---|---|---|
|`point`|`Vector2`|Tọa độ **tâm** của hình tròn kiểm tra|
|`radius`|`float`|**Bán kính** của hình tròn|
|`layerMask`|`LayerMask`|**Layer** cần kiểm tra|

**Kiểu dữ liệu trả về:** `Collider2D` — trả về Collider đầu tiên tìm thấy, hoặc `null` nếu không có gì.

==Ví dụ minh họa==
```C#
public Transform attackPoint;   // Tâm vùng tấn công (gắn vào tay nhân vật)
public float attackRadius;      // Bán kính vùng tấn công, ví dụ: 0.5f
public LayerMask enemyLayer;    // Layer "Enemy"

// Kiểm tra kẻ địch trong vùng tấn công khi nhân vật tấn công
bool hitEnemy = Physics2D.OverlapCircle(
    attackPoint.position,   // Tâm hình tròn
    attackRadius,           // Bán kính
    enemyLayer              // Chỉ kiểm tra với Layer "Enemy"
) != null;
```

==Ứng dụng thực tế trong làm game== `OverlapCircle` phù hợp hơn `OverlapBox` khi vùng kiểm tra cần **toả đều theo mọi hướng**:

- **Vùng tấn công cận chiến:** Kiểm tra kẻ địch nằm trong tầm đánh của nhân vật (hình tròn tự nhiên hơn hình chữ nhật cho việc tấn công).
- **Vùng phát hiện kẻ địch (Detection Range):** Kiểm tra xem người chơi có bước vào phạm vi nhìn của kẻ địch không.
- **Nhặt vật phẩm tự động:** Kiểm tra xem vật phẩm có nằm đủ gần nhân vật để tự động hút vào không.

==Gizmo đi kèm==
```C#
void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
}
```

==So sánh OverlapBox vs OverlapCircle==

|                    | OverlapBox                                       | OverlapCircle                               |
| ------------------ | ------------------------------------------------ | ------------------------------------------- |
| Hình dạng          | Hình chữ nhật                                    | Hình tròn                                   |
| Tham số kích thước | `Vector2 size` (rộng × cao)                      | `float radius` (bán kính)                   |
| Phù hợp cho        | Kiểm tra mặt đất, trần nhà, tường (bề mặt phẳng) | Vùng tấn công, phát hiện, thu hút (toả đều) |

3. ==**Physics2D.Raycast**==: dùng để **bắn một tia vô hình theo hướng nhất định** và kiểm tra xem tia đó có trúng Collider nào không. Hình dung như bắn một **tia laser** từ một điểm trong không gian — tia laser đi thẳng và báo lại nó chạm vào cái gì, ở đâu, và ở khoảng cách bao nhiêu.

==Cú pháp và giải thích tham số== **Cú pháp:** `Physics2D.Raycast(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask)`

|Tham số|Kiểu dữ liệu|Ý nghĩa|
|---|---|---|
|`origin`|`Vector2`|**Điểm xuất phát** của tia|
|`direction`|`Vector2`|**Hướng** bắn tia (thường dùng `Vector2.down`, `Vector2.left`...)|
|`distance`|`float`|**Độ dài tối đa** của tia. Để `Mathf.Infinity` nếu muốn bắn vô hạn|
|`layerMask`|`LayerMask`|**Layer** cần kiểm tra|

**Kiểu dữ liệu trả về:** `RaycastHit2D` — một struct chứa **thông tin chi tiết** về điểm va chạm (khác với Overlap chỉ trả về `Collider2D`).

==Thông tin trong RaycastHit2D== Đây là điểm mạnh của Raycast so với Overlap — bạn biết được **chi tiết va chạm**:

|Thuộc tính|Ý nghĩa|
|---|---|
|`hit.collider`|Collider bị trúng (kiểm tra `!= null` để biết có trúng gì không)|
|`hit.distance`|Khoảng cách từ điểm xuất phát đến điểm va chạm|
|`hit.point`|Tọa độ chính xác của điểm va chạm trong không gian|
|`hit.normal`|Hướng pháp tuyến tại điểm va chạm (vuông góc với bề mặt)|

==Ví dụ minh họa==

```C#
public LayerMask groundLayer;

void Update()
{
    // Bắn tia thẳng xuống từ vị trí nhân vật, dài tối đa 1f
    RaycastHit2D hit = Physics2D.Raycast(
        transform.position,     // Điểm xuất phát
        Vector2.down,           // Hướng xuống
        1f,                     // Độ dài tia
        groundLayer             // Chỉ kiểm tra Layer "Ground"
    );

    // Kiểm tra có trúng gì không
    if (hit.collider != null)
    {
        bool isGrounded = true;
        float distanceToGround = hit.distance; // Khoảng cách đến mặt đất
    }
}
```

==Vẽ tia Raycast bằng Debug (thay Gizmo)== Raycast không dùng `Gizmos.DrawWireCube` như Overlap mà dùng `Debug.DrawRay` để thấy tia trong Scene:

```C#
void Update()
{
    // Vẽ tia màu đỏ dài 1f xuống dưới (chỉ thấy trong Scene view khi Play)
    Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);
}
```

==Ứng dụng thực tế trong làm game== Raycast được dùng khi bạn cần biết **thông tin chi tiết** về điểm va chạm, không chỉ đơn thuần "có va chạm hay không":

- **Phát hiện mặt đất dốc (Slope):** Dùng `hit.normal` để biết góc nghiêng của bề mặt mà nhân vật đang đứng, từ đó điều chỉnh chuyển động cho tự nhiên.
- **Hệ thống ngắm bắn:** Bắn tia từ nòng súng theo hướng nhắm, lấy `hit.point` làm điểm nổ hoặc điểm trúng đạn.
- **Kiểm tra tầm nhìn kẻ địch (Line of Sight):** Bắn tia từ kẻ địch đến người chơi — nếu tia bị chặn bởi tường thì kẻ địch "không nhìn thấy" người chơi.
- **Leo tường (Wall Slide):** Bắn tia sang ngang để phát hiện tường và khoảng cách đến tường.

==So sánh Overlap vs Raycast — khi nào dùng cái nào==

|                    | **Overlap** (Box/Circle)                       | Raycast                                      |
| ------------------ | ---------------------------------------------- | -------------------------------------------- |
| Hình dạng kiểm tra | Vùng diện tích (hình chữ nhật / tròn)          | Tia thẳng                                    |
| Thông tin trả về   | Chỉ biết "có Collider không"                   | Biết thêm khoảng cách, điểm chạm, góc bề mặt |
| Phù hợp cho        | isGrounded, vùng tấn công, phát hiện theo vùng | Bắn đạn, tầm nhìn, phát hiện mặt dốc         |
| Gizmo / Debug      | `Gizmos.DrawWireCube` / `DrawWireSphere`       | `Debug.DrawRay`                              |


---

==Còn các hàm khác trong Physics2D (biết để tham khảo)== Unity còn cung cấp các biến thể khác, ít dùng hơn trong platformer 2D cơ bản:

- **`OverlapCapsule`** — kiểm tra vùng hình viên con nhộng (2 đầu tròn, thân thẳng). Hữu ích khi nhân vật có CapsuleCollider2D.
- **`Linecast`** — giống Raycast nhưng bạn chỉ định **điểm đầu và điểm cuối** cụ thể thay vì hướng + khoảng cách.
- **`BoxCast` / `CircleCast` / `CapsuleCast`** — lai giữa Cast và Overlap: bắn cả **hình dạng** (không phải tia) theo một hướng và trả về thông tin va chạm chi tiết như Raycast.