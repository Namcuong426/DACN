document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('input').forEach(function (input) {
        input.addEventListener('keypress', function (event) {
            if (event.keyCode === 13) {
                event.preventDefault(); // Ngăn chặn hành động mặc định của nút Enter
            }
        });
    });
});
function openmodal(url) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                document.querySelector("#exampleModalCenter .modal-content").innerHTML = xhr.responseText;
                var modal = new bootstrap.Modal(document.getElementById('exampleModalCenter'));
                modal.show();
            } else {
                console.error('Request failed:', xhr.status);
            }
        }
    };
    xhr.send();
}
function onSaveAccount() {
    var pass = document.getElementById('password').value;
    var passC = document.getElementById('passwordconfirm').value;
    if (pass == passC) {
        var formData = new FormData();
        formData.append('Id', $('#accountIdInput').val());
        formData.append('EmailAddress', $('#exampleInputEmail1').val());
        formData.append('Password', $('#password').val());
        formData.append('PasswordConfirm', $('#passwordconfirm').val());
        $.ajax({
            url: '/Admin/Account/CreateAccount',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (!data.success) {
                    // Nếu không thành công, hiển thị thông điệp lỗi trong modal
                    document.getElementById("errcheck").innerHTML = data.message;
                    $('#myModalContent').html(data.message);
                    $('#myModal').modal('show');
                } else {
                    window.location.reload();
                }
                $('#myModal').on('hidden.bs.modal', function (e) {
                    data.message = "";
                });
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi
            }
        });
    }
    else {
        document.getElementById("errcheck").innerHTML = "Password And PasswordConfirm Not Valid"
    }
}