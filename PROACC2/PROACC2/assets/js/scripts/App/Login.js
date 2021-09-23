/*Login Script start*/
function callme(ID) {
    $(ID).html(
        `<span class="fa fa-cog fa-spin" role="status" aria-hidden="true"></span> Loading...`
    );
    $(ID).css('font-size', 'large !important');
}
function CheckUserNameEmailAvailabiltiy(Name, EmailId, Url) {
    var _name = Name;
    var _email = EmailId;
    var _status = true;
    $.ajax({
        type: 'Get',
        url: Url,
        data: {
            Name: _name,
            EmailId: _email
        },
        dataType: "json",
        async: false,
        success: function (response) {
            if (response == false) {
                _status = false;
            }
        }
    })
    return _status;
}
function forgotPassword(UrlFwd, Name, EmailId) {
    $.ajax({
        type: 'POST',
        url: UrlFwd,
        data: {
            Name: Name,
            EmailId: EmailId
        },
        success: function (response) {
            if (response == "success") {
                Notiflix.Notify.Success('Reset Password Link Sent Successfully..Kindly Check Email!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                ResetForgot();
                location.reload();
            }
            else {
                alert("validation Fail");
            }
        }
    });
}

function checkEmail(emailId) {
    //var emailId = $("#EmailId").val().trim();
    var regex = /^([\w-\.]+)@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (!regex.test(emailId)) {
        return false;
    }
    else {
        return true;
    }
}

/*Login Script End*/