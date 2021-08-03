
Notiflix.Notify.Init({
    width: '280px',
    position: 'left-bottom',
    opacity: 1,
    fontFamily: "sans-serif",
    fontSize: "15px",
    distance: "2px",
    borderRadius: "10px",
});

/******* LAYOUT START ******/
function UploadPhoto(URL, RL_Url,sht) {
    var file = $("#fileUpload").val();
    var data = $("#fileUpload").get(0);
    var files = [data.files];
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        if (files[i][0] != null)
            fileData.append(i, files[i][0]);
    }

    if (file == "") {
        $("#lblPhotoUpload").html("Please Select Photo").show();
    }
    else {
        $("#lblPhotoUpload").html(" ").show();

    }
    if (fileuploadfile() == false) {
        $("#fileUpload").val("");
        $("#lblPhotoUpload").html("Size should be less than 2 Mb").show();
    }
    else {
        $("#lblPhotoUpload").html(" ").show();

    }
    if (fileExtension() == false) {
        $("#fileUpload").val("");
        $("#lblPhotoUpload").html("Upload Only jpg, jpeg, png").show();
    }
    else {
        $("#lblPhotoUpload").html(" ").show();

        $.ajax({
            type: "POST",
            url: URL,
            data: fileData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response == "success") {
                    $("#UploadFile").modal('hide');
                    $(".modal-backdrop").remove();
                    $("#fileUpload").val("");
                    $("#DrpRefresh").hide();
                    Notiflix.Report.Success(
                        'Profile Update',
                        'Photo <strong>uploaded </strong>successfully..!',
                        'Close',
                        function () {
                            load_Profile(RL_Url, sht);
                        },
                    );
                    //Notiflix.Notify.Success('Photo <strong>uploaded </strong>successfully..!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, load_Profile(RL_Url, sht), });
                }
                else {
                    Notiflix.Notify.Info('Unable to Process Request..File Not Uploaded!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });

                }
            }
        });
    }
    //Reload(RL_Url);
    setTimeout(load_Profile(RL_Url, sht), 3000);
}

function PwdUpdate(URL, OLD_Pwd_URL) {
    var _newPassword = $("#txtNewPassword").val().trim();
    var _cnfPassword = $("#txtCnfPassword").val().trim();
    var _oldpassword1 = $("#txtOldPassword").val().trim();

    var _Pwd = _Newpwd = _Cnfpwd = false;

    var model = {
        Password: _newPassword
    }
    if (_oldpassword1 == "") {
        _Pwd = true;
        $("#Oldpwd").show();
        $("#lblOldPassword").html("Please enter the Old Password").show();
        $("#txtOldPassword").val("");
    }
    else {
        $("#Oldpwd").hide();
        $("#lblOldPassword").html("").show();
    }
    if (oldpasswordcheck(OLD_Pwd_URL) == false) {
        if (_oldpassword1 == "") {
            _Pwd = true;
            $("#Oldpwd").show();
            $("#lblOldPassword").html("Please enter the Old Password").show();
            $("#txtOldPassword").val("");
        }
        else {
            $("#Oldpwd").show();
            $("#lblOldPassword").html("Please enter the Correct Password").show();
            $("#txtOldPassword").val("");
            $("#txtNewPassword").val("");
            $("#txtCnfPassword").val("");
        }
    }
    else {
        if (_oldpassword1 == "") {
            $("#Oldpwd").show();
            $("#lblOldPassword").html("Please enter the Old Password").show();
            $("#txtOldPassword").val("");
        }
        else {
            $("#Oldpwd").hide();
            $("#lblOldPassword").html("").show();
        }
    }

    if (_newPassword == "") {
        _Newpwd = true;
        $("#Newpwd").show();
        $("#txtNewPassword").val("");
        $("#lblNewPassword").html("Please enter the New Password").show();

    }
    else {
        $("#Newpwd").hide();
        $("#lblNewPassword").html("").show();
    }

    if (TaskPwd() == false) {
        if (_newPassword == "") {
            $("#Newpwd").show();
            $("#txtNewPassword").val("");
            $("#lblNewPassword").html("Please enter the New Password").show();

        }
        else {
            $("#Newpwd").show();
            $("#txtNewPassword").val("");
            $("#txtCnfPassword").val("");
            $("#lblNewPassword").html("Password should contain one character, Special char and number").show();
        }
    }
    else {
        if (_newPassword == "") {
            $("#Newpwd").show();
            $("#txtNewPassword").val("");
            $("#lblNewPassword").html("Please enter the New Password").show();

        }
        else {
            $("#Newpwd").hide();
            $("#lblNewPassword").html("").show();
        }

    }
    if (oldnewpassword() == false) {
        if (_newPassword == "") {
            $("#Newpwd").show();
            $("#txtNewPassword").val("");
            $("#lblNewPassword").html("Please enter the New Password").show();
        }
        else {
            $("#Newpwd").show();
            $("#txtNewPassword").val("");
            $("#lblNewPassword").html("Choose another password except old").show();
        }
    }
    else {
        if (_newPassword == "") {
            $("#Newpwd").show();
            $("#txtNewPassword").val("");
            $("#lblNewPassword").html("Please enter the New Password").show();

        }
        else {
            $("#Newpwd").hide();
            $("#lblNewPassword").html("").show();
        }
    }
    if (_cnfPassword == "") {
        _Cnfpwd = true;
        $("#Cnfpwd").show();
        $("#lblCnfPassword").html("Please enter the Confirm Password").show();
    }
    else {
        $("#Cnfpwd").hide();
        $("#lblCnfPassword").html("").show();

    }
    if (cnfpassword() == false) {
        $("#Cnfpwd").show();
        $("#txtCnfPassword").val("");
        $("#lblCnfPassword").html("Password Does Not Match Kindly Check").show();
    }
    else {
        if (_cnfPassword == "") {
            $("#Cnfpwd").show();
            $("#lblCnfPassword").html("Please enter the Confirm Password").show();
        }
        else {
            $("#Cnfpwd").hide();
            $("#lblCnfPassword").html("").show();

        }
    }
    if (_Newpwd == false && _Pwd == false && _Cnfpwd == false && TaskPwd() == true && cnfpassword() == true && oldnewpassword() == true && oldpasswordcheck() == true) {
        $.ajax({
            type: "POST",
            url: URL,
            data: model,
            success: function (response) {
                if (response == "success") {
                    //Notiflix.Notify.Success('Password Updated successfully..Kindly Login Again!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });

                    //setTimeout(Redirect, 3000);
                    ResetUpdtPwd();
                    location.href = $("#RedirectToHome").val();
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
}
function ResetUpdtPwd() {
    $("#Oldpwd").hide();
    $("#Newpwd").hide();
    $("#Cnfpwd").hide();
    $("#txtNewPassword").val("");
    $("#txtCnfPassword").val("");
    $("#lblOldPassword").html("").show();
    $("#lblNewPassword").html("").show();
    $("#lblCnfPassword").html("").show();

    $("#UpdatePwd").modal('hide');
    $('.modal-backdrop').remove();

}
function oldpasswordcheck(URL) {
    var _oldpassword = $("#txtOldPassword").val().trim();
    var _status = true;
    $.ajax({
        type: 'GET',
        data: { password: _oldpassword },
        url: URL,
        async: false,
        success: function (response) {
            if (response == "error") {
                _status = false;
            }
            else {
                _status = true;
            }
        }
    });
    return _status;
}

function oldnewpassword() {
    var _password1 = $("#txtNewPassword").val().trim();
    var _oldpassword1 = $("#txtOldPassword").val().trim();
    if (_password1 == _oldpassword1) {
        return false;

    }
    else {
        return true;
    }
}

function cnfpassword() {
    var str = $("#txtNewPassword").val().trim();
    var str1 = $("#txtCnfPassword").val().trim();
    if (str != str1) {
        $("#txtCnfPassword").val("")
        return false;
    }
    else {
        return true;
    }
}

function TaskPwd() {
    var status = true;
    var str = $("#txtNewPassword").val();
    var filter = /^(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[^\w\s]).{6,8}$/
    if (str.length < 6) {
        $("#lblNewPassword").html("Password should contain 6-8 characters").show();
        status = false;
    }
    else if (!filter.test(str)) {
        $("#lblNewPassword").html("Password should contain at least one character,Special char and number").show();
        status = false;
    }
    else {
        $("#lblNewPassword").html("").show();
        status = true
    }
    return status
}

function fileuploadfile() {
    var file_size = $('#fileUpload')[0].files[0].size;
    var status = true;
    if (file_size > 5097152) {
        status = false;
    }
    else {
        status = true;
    }
    return status;

}

function fileExtension() {
    var fname = $('#fileUpload')[0].files[0].name;
    var fextension = fname.substring(fname.lastIndexOf('.') + 1).toLowerCase();
    var validExtensions = ["jpg", "jpeg", "png"];
    var status = true;
    if ($.inArray(fextension, validExtensions) == -1) {
        status = false;
    }
    else {
        status = true;
    }
    return status;

}

//function Reload(URL) {
//    $.ajax({
//        type: 'GET',
//        url: URL,
//        async: true,
//        dataType: 'json',
//        success: function (result) {
//            if (result != null) {

//                document.getElementById('UserDisp2').style.opacity = 1;
//                $("#UserDisp2,#UserDisp").attr('src', result);
//                $("#UserDisp2,#UserDisp").show();
//                document.getElementById('profileVisibilty').style.opacity = 0;
//                document.getElementById('profileVisibilty2').style.opacity = 0;
//                //document.getElementById('camera2').style.opacity = 0;

//            }
//            else {
//                //$("#UserDisp").hide();
//                $("#UserDisp2,#UserDisp").attr('src', "");
//                document.getElementById('profileVisibilty2').style.opacity = 1;
//                document.getElementById('profileVisibilty').style.opacity = 1;
//                document.getElementById('UserDisp2').style.opacity = 0;
//            }
//        }
//    });
//}

function load_Profile(URL, Sht, col) {
    
    $.ajax({
        type: 'GET',
        url: URL,
        async: true,
        data: { col: col },
        dataType: 'json',
        success: function (result) {
           
            $('#UserProfile').html("");
            $('#_avatar').html("");
            if (result == "false") {

                $('#UserProfile').addClass('profile-dropdown');

                var tag = '<span class="_cla">_Letter</span>';
                tag = tag.replace("_Letter", Sht);
                $('#UserProfile').html(tag);

                var Av_Tag = '<div class="profile-dropdown-avatar"><span>_Letter</span></div><span class="fa fa-camera c-avatar-Camara"></span>';
                Av_Tag = Av_Tag.replace("_Letter", Sht);
                $('#_avatar').html(Av_Tag);
                $('#UserProfile').addClass(col);
                $('.profile-dropdown-avatar').addClass(col);


            }
            else {
                result = result.replace('~', "..");
                var img = '<img class="c-avatar-img-Click" src="_Path">';
                var img_av = '<img class="c-avatar-img-menu" src="_Path"><span class="fa fa-camera c-avatar-Camara"></span>';

                img = img.replace("_Path", result);
                img_av = img_av.replace("_Path", result);
                $('#UserProfile').removeClass();
                $('#UserProfile').addClass('nav-link dropdown-toggle link')
                $('#UserProfile').html(img);
                $('#_avatar').html(img_av);
            }


        }
    });
}
function funcUpload() {
    var clicks = $('.upload').data('clicked-times');
    $('.upload').data('clicked-times', ++clicks);
    if (clicks % 2 == 1) {
        document.getElementById("DrpRefresh").style.opacity = 1;
        $("#DrpRefresh").show();
        event.stopPropagation();
    }
    else {
        $("#DrpRefresh").hide();
    }
}

function random(min, max) {
    var r = Math.round(Math.random() * (max - min) + parseInt(min));
    console.log(r);
    return r;
}
function random(min, max, ex) {
    r = Math.round(Math.random() * (max - min) + parseInt(min));
    if (r === ex) r++;
    if (r > max) r = r - 2;
    return r;
}
function getSColor(n) {
    switch (n) {
        case 1: color = 1; return "green-sea";
        case 2: color = 2; return "nephritis";
        case 3: color = 3; return "belize-hole";
        case 4: color = 4; return "wisteria";
        case 5: color = 5; return "orange";
        case 6: color = 6; return "pumpkin";
        case 7: color = 7; return "pomegranate";
    }
}




    //function ImagePixel() {
    //    debugger;
    //    var status = true;
    //    var data = $("#fileUpload").get(0);
    //    if (data) {
    //        var reader = new FileReader();
    //        reader.readAsDataURL(fileUpload.files[0]);
    //        reader.onload = function (e) {
    //            var image = new Image();

    //            image.src = e.target.result;

    //            image.onload = function () {
    //                var height = this.height;
    //                var width = this.width;

    //                if (width == 50 && height == 50) {
    //                    status = false;
    //                }
    //                else {
    //                    status = true;
    //                }
    //            };
    //        }
    //    }
    //    return status;

    //}

/******* LAYOUT END ******/


/******Instance START********/
function ResetProject() {
    $('#addProject').modal('hide');
    $('.modal-backdrop').remove();

    $("#Maxchar").show();
    $("#asterisk").show();
    $("#asterisk1").show();
    $("#asterisk2").show();
    $("#asterisk3").show();
    $("#Insstar").show();
    $("#Prostar").show();
    $("#updtInsta").show();

    $("#txtProjectName").val('');
    $("#drpProjectManager").val(0);
    $("#drpScenario").val(0);
    $("#drpCustomer").val(0);

    $("#lblProjectName").html("").show();
    $("#lblCustomerName").html("").show();
    $("#lblProjectManager").html("").show();
    $("#lblScenerio").html("").show();
    $("#desc").val('');


}

function ResetInstance() {
    $("#addinstance").modal('hide');
    $('.modal-backdrop').remove();

    $("#Maxchar").hide();
    $("#asterisk").hide();
    $("#asterisk1").hide();
    $("#asterisk2").hide();
    $("#asterisk3").hide();
    $("#Insstar").hide();
    $("#Prostar").hide();
    $("#updtInsta").hide();

    $("#txtInstanceName").val('')
    $("#drpProject").val(0)
    $("#description").val('')
    $("#lblInstanceName").html("").show();
    $("#lblProject").html("").show();
    $("#lblDescription").html("").show();
    $("#Maxchar").hide();


}
/******Instance END********/