function SessionStorage(CustId,PhId,PrjId,InsId) {
    sessionStorage.setItem("Proj", PrjId);
    sessionStorage.setItem("Ins", InsId);
    sessionStorage.setItem("PhId", PhId);
    sessionStorage.setItem("CustId", CustId);
}

//function Loading_On() {
//    $("#load").css("display", "block");
//}
//function Loading_Off() {
//    $("#load").css("display", "none");
//    $('[data-toggle="tooltip"]').tooltip();
//}
/*==================================================================Detail Report Page Start=========================================================================== */

function GlobalBindInstance(PrjId, Url) {
    
    if (PrjId == 0) {
        var insta = $('#ProjInstance');
        insta.empty();
        insta.append($('<option/>', {
            value: 0,
            text: 'Select Instance'
        }));
        $("#DwnldDetailResource").hide();

    }
    else {
        $.ajax({
            url: Url,
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            data: { id: PrjId },
            success: function (data) {
                var state = $('#ProjInstance');
                state.empty();
                state.append($('<option/>', {
                    value: 0,
                    text: 'Select Instance'
                }));
                $.each(data, function (item, value) {
                    $('#ProjInstance').append($("<option class='text-capitalize' value='" + value.Instance_id + "'>" + value.InstanceName + "</option>"));
                });
                $('.show_ins').addClass('show_data').removeClass('show_ins');
            },
            error: function (err) {
                alert(err);
            }
        });
    }
}


function BindingProject_Detail(CustId, CustUrl) {
    if (CustId == 0) {
        var Prjt = $('#drpProject');
        Prjt.empty();
        Prjt.append($('<option/>', {
            value: 0,
            text: 'Select Project'
        }));

        var insta = $('#ProjInstance');
        insta.empty();
        insta.append($('<option/>', {
            value: 0,
            text: 'Select Instance'
        }));
    }
    else {
        $.ajax({
            url:CustUrl ,
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            data: { id: CustId },
            success: function (data) {
                var state = $('#drpProject');
                state.empty();
                state.append($('<option/>', {
                    value: 0,
                    text: 'Select Project'
                }));
                var insta = $('#ProjInstance');
                insta.empty();
                insta.append($('<option/>', {
                    value: 0,
                    text: 'Select Instance'
                }));
                $.each(data, function (item, value) {
                    $('#drpProject').append($("<option class='text-capitalize' value='" + value.ProjectId + "'>" + value.ProjectName + "</option>"));
                });
                $('.show_prj').addClass('show_data').removeClass('show_prj');

            },
            error: function (err) {
                alert(err);
            }
        });
    }
}
function DetailReport_Load(Id,url) {
    var id = Id;

    $.ajax({
        type: 'GET',
        url:url ,
        data: {
            id: id
        },

        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            $('.reportTable tbody').remove();
            var tbody = '<tbody>'
            var tr = '<tr class="tr_Effect">';
            var ts = '';
            if (response.length > 0) {
                $.each(response, function (index, val) {

                    var bb = '<td id="BB"><span class="text-capitalize" data-toggle="tooltip" title="' + val.BuildingBlock + '">' + val.BuildingBlock + '</span></td>';
                    bb += '<td id="PH"><span class="text-capitalize" data-toggle="tooltip" title="' + val.Phase + '">' + val.Phase + '</span></td>';
                    bb += '<td id="AT"><span class="text-capitalize" data-toggle="tooltip" title="' + val.Task + '">' + val.Task + '</span></td>';
                    bb += '<td id="AA"><span class="text-capitalize" data-toggle="tooltip" title="' + val.Role + '">' + val.Role + '</span></td>';
                    bb += '<td id="DL"><span class="text-capitalize" data-toggle="tooltip" title="' + val.Delay_occurred_Report + '">' + val.Delay_occurred_Report + '</span></td>';
                    bb += '<td id="OW"><span class="text-capitalize" data-toggle="tooltip" title="' + val.Owner + '">' + val.Owner + '</span></td>';
                    bb += '<td><span class="text-capitalize" data-toggle="tooltip" title="' + val.Status + '">' + val.Status + '</span></td>';
                    bb += '<td id="EST"><span class="text-capitalize" data-toggle="tooltip" title="' + val.EST_hrs + '">' + val.EST_hrs + '</span></td>';
                    bb += '<td id="PSD"><span class=" text-capitalize" data-toggle="tooltip" title="' + val.PlanedDate + '">' + val.PlanedDate + '</span></td>';
                    bb += '<td id="ASD"><span class="text-capitalize" data-toggle="tooltip" title="' + val.ActualDate + '">' + val.ActualDate + '</span></td>';
                    bb += '<td id="PED"><span class="text-capitalize" data-toggle="tooltip" title="' + val.PlanedEn_Date + '">' + val.PlanedEn_Date + '</span></td>';
                    bb += '<td id="AED"><span class="text-capitalize" data-toggle="tooltip" title="' + val.ActualEn_Date + '">' + val.ActualEn_Date + '</span></td>';
                    ts = ts + tr + bb + '</tr>';


                });
                Notiflix.Notify.Success('Record <Strong>successfully </strong>loaded..!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                tbody = tbody + ts + '</tbody>';
                $('.reportTable').append(tbody);
                $(".custom-select,#testselect").removeAttr('disabled');
                $(".custom-select").removeClass('disabled');
                $('.show_drpChk').addClass('show_data').removeClass('show_drpChk');

                $("#export").show();
            }
            else {
                Notiflix.Notify.Info('No <Strong>Record </strong>Found..!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                var bb2 = '<td colspan="12" class="text-center">' + "<strong>No Record Found</strong>" + '</td>';
                tr = tr + bb2 + '</tr>';
                tbody = tbody + tr + '</tbody>';
                $('.reportTable').append(tbody);
                $(".custom-select,#testselect").attr('disabled', 'disabled');
                $("#export").hide();

            }

        }
    });
    $("#DwnldDetailResource").show();
}

/*==================================================================Detail Report Page End=========================================================================== */

/*==================================================================Resource Allocation Transaction Page Start=========================================================================== */

//function BindInstance_transaction(PrjId, Url) {
//    if (PrjId == 0) {
//        var insta = $('#ProjInstance');
//        insta.empty();
//        insta.append($('<option/>', {
//            value: 0,
//            text: 'Select Instance'
//        }));
//    }
//    else {
//        $.ajax({
//            url: Url,
//            dataType: "json",
//            async: false,
//            contentType: "application/json; charset=utf-8",
//            data: { id: PrjId },
//            success: function (data) {
//                var state = $('#ProjInstance');
//                state.empty();
//                state.append($('<option/>', {
//                    value: 0,
//                    text: 'Select Instance'
//                }));
//                $.each(data, function (item, value) {
//                    $('#ProjInstance').append($("<option value='" + value.Instance_id + "'>" + value.InstanceName + "</option>"));
//                })
//                $('.show_ins').addClass('show_data').removeClass('show_ins');

//            },
//            error: function (err) {
//                alert(err);
//            }
//        });
//    }
//}

function LoadResource_Allocation(PhId, InsId, ResourceUrl, GetUserUrl) {
    var RoleList = GetUserDropDown(GetUserUrl);
                        
    $("#BulkAllocation").show();
    $.ajax({
        url: ResourceUrl,
        async: false,
        data: { PhaseId: PhId, InstanceId: InsId, first: true },
        success: function (response) {
            $('.mytable tbody').remove();
            var tbody = '<tbody>';
            var tr = '<tr>';
            var Unassingned = 0;

            if (response.length > 0) {
                $.each(response, function (key, val) {
                    //console.log(val.UserID);
                    if (val.UserID == '00000000-0000-0000-0000-000000000000') {
                        Unassingned = Unassingned + 1;
                    }
                    var tr2 = '<tr class="tr_Effect" id="' + val.Id + '">';
                    var bb = '<td class="TextRap"><span class="text-capitalize" data-toggle="tooltip" title="">' + ++key + '.</span></td>';
                    if (val.Task.length > 9) {
                        var Task = val.Task.substring(0, 10) + "...";
                        bb += '<td class="TextRap ToggleResource head content"><span class="text-capitalize" data-original-title="" data-toggle="popover"   id="slideToggletask" style="cursor:pointer" rel="popover" data-placement="bottom"  data-trigger="hover" data-content="' + val.Task + '">' + Task + '</span ></td>';
                    }
                    else {
                        bb += '<td class="TextRap ToggleResource head content"><span class="text-capitalize" data-original-title="" data-toggle="popover"  id="slideToggletask" style="cursor:pointer" rel="popover" data-placement="bottom"  data-trigger="hover" data-content="' + val.Task + '">' + val.Task + '</span ></td>';
                    }
                    bb += '<td class="TextRap"><span class="text-capitalize"  data-placement="top" data-toggle="tooltip" title="' + val.BuildingBlock + '">' + val.BuildingBlock + '</span></td>';
                    bb += '<td class="TextRap"><span class="text-capitalize"  data-placement="top" data-toggle="tooltip" title="' + val.ApplicationArea + '">' + val.ApplicationArea + '</td>';
                    bb += '<td class="TextRap"><span class="text-capitalize RoleId"  data-placement="top" data-toggle="tooltip" title="' + val.Role + '">' + val.Role + '</td>';
                    bb += '<td><select class="form-control drpOwner drpTextHeight" id="dr_' + val.Id + '"><option value="0">Unassigned</option><option id="UserID">Select</option></select></td>';
                    if (val.Status == "Yet To Start") {
                        bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                    }
                    else if (val.Status == "On Hold") {
                        bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                    }
                    else if (val.Status == "In Progress") {
                        bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                    }
                    else if (val.Status == "Not Applicable") {
                        bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                    }
                    else if (val.Status == "Completed") {
                        bb += '<td class="TextRap"><span class="text-capitalize text-success font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-success font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                    }
                    bb += '<td class="TextRap hidden"><span class="text-capitalize" title="' + val.Task + '">' + val.Task + '</span ></td>';
                    bb += '<td class="TextRap"><span class="toolid" hidden>' + val.Id + ' </span></td>';
                    tr = tr + tr2 + bb + '</tr>' + '</tr>';

                });
            }
            else {

                if ($('#Tab_Resource').hasClass('active')) {
                    Notiflix.Confirm.Show(
                        'Confirm',
                        'Are you sure to Pull the data from Activity?',
                        'Yes', 'No',
                        function () {
                            $.ajax({
                                type: "POST",
                                url: ResourceUrl,
                                data: { PhaseId: PhId, InstanceId: InsId, first: false },
                                dataType: "json",
                                async: false,
                                success: function (response) {
                                    $('.mytable tbody').remove();
                                    var tbody = '<tbody>';
                                    var tr = '<tr>';
                                    if (response.length > 0) {
                                        $.each(response, function (key, val) {
                                            if (val.UserID == '00000000-0000-0000-0000-000000000000') {
                                                Unassingned = Unassingned + 1;
                                            }
                                            var tr2 = '<tr class="tr_Effect" id="' + val.Id + '">';
                                            var bb = '<td class="TextRap"><span class="text-capitalize" data-toggle="tooltip" title="">' + ++key + '.</span></td>';
                                            if (val.Task.length > 9) {
                                                var Task = val.Task.substring(0, 10) + "...";
                                                bb += '<td class="TextRap ToggleResource head content"><span class="text-capitalize" data-original-title="Activity/Task" data-toggle="popover"   id="slideToggletask" style="cursor:pointer" rel="popover" data-placement="bottom"  data-trigger="hover" data-content="' + val.Task + '">' + Task + '</span ></td>';
                                            }
                                            else {
                                                bb += '<td class="TextRap ToggleResource head content"><span class="text-capitalize" data-original-title="Activity/Task" data-toggle="popover"  id="slideToggletask" style="cursor:pointer" rel="popover" data-placement="bottom"  data-trigger="hover" data-content="' + val.Task + '">' + val.Task + '...</span ></td>';
                                            }
                                            bb += '<td class="TextRap"><span class="text-capitalize"  data-placement="top" data-toggle="tooltip" title="' + val.BuildingBlock + '">' + val.BuildingBlock + '</span></td>';
                                            bb += '<td class="TextRap"><span class="text-capitalize"  data-placement="top" data-toggle="tooltip" title="' + val.ApplicationArea + '">' + val.ApplicationArea + '</td>';
                                            bb += '<td class="TextRap"><span class="text-capitalize RoleId"  data-placement="top" data-toggle="tooltip" title="' + val.Role + '">' + val.Role + '</td>';
                                            bb += '<td><select class="form-control drpOwner drpTextHeight" id="dr_' + val.Id + '"><option value="0">Unassigned</option><option id="UserID">Select</option></select></td>';
                                            if (val.Status == "Yet To Start") {
                                                bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                                            }
                                            else if (val.Status == "On Hold") {
                                                bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                                            }
                                            else if (val.Status == "In Progress") {
                                                bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                                            }
                                            else if (val.Status == "Not Applicable") {
                                                bb += '<td class="TextRap"><span class="text-capitalize text-secondary font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-secondary font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                                            }
                                            else if (val.Status == "Completed") {
                                                bb += '<td class="TextRap"><span class="text-capitalize text-success font-weight-bold"  data-placement="top" data-toggle="tooltip" title="' + val.Status + '"><i class="fa fa-circle text-success font-weight-normal _trackcircle" aria-hidden="true"></i>&nbsp;' + val.Status + '</span></td>';
                                            }
                                            bb += '<td class="TextRap hidden"><span class="text-capitalize" title="' + val.Task + '">' + val.Task + '</span ></td>';
                                            bb += '<td class="TextRap"><span class="toolid" hidden>' + val.Id + ' </span></td>';
                                            tr = tr + tr2 + bb + '</tr>' + '</tr>';
                                        });
                                    }
                                    else {
                                        var tr2 = '<tr class="tr_Effect" id="">';
                                        var bb = '<td colspan="8" style="text-align:center"> No Records Found</td>';
                                        tr = tr + tr2 + bb + '</tr>' + '</tr>';
                                    }
                                    tbody = tbody + tr + '</tbody>';
                                    $('.mytable').append(tbody);
                                    $('#Unassigned_ct').text(Unassingned);
                                    //RefreshResourceAllocation();

                                    if (response.length > 0) {
                                        for (var i = 0; i < response.length; i++) {
                                            //UpdateUserByRole(response[i].RoleID, response[i].UserID, InsId);
                                            UpdateUserRole(RoleList, response[i].RoleID, response[i].UserID);
                                        }
                                    }
                                }
                            });
                        }, function () {
                            ResetInstProject();
                            var result = $("#drpPhase :selected").text();
                            $("#CurrentPhase").text(result);
                        }
                    );
                }

            }
            tbody = tbody + tr + '</tbody>';
            $('.mytable').append(tbody);

            $('#Unassigned_ct').text(Unassingned);
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    UpdateUserRole(RoleList, response[i].RoleID, response[i].UserID);
                    //UpdateUserByRole(response[i].RoleID, response[i].UserID, InsId);
                    if (response[i].Status == 'Completed' || response[i].Status == 'Not Applicable') {
                        $('#dr_' + response[i].Id).prop("disabled", true);
                    }
                    else {
                        $('#dr_' + response[i].Id).prop("disabled", false);
                    }

                }
            }
        }

    });
}

function GetUserDropDown(GetUserUrl) {
        
    var InstanceId = GetUserUrl.substring(GetUserUrl.lastIndexOf('=') + 1);

    var Parameters = {};
    if (InstanceId != 0) {
        $.ajax({
            type: "POST",
            url: GetUserUrl,
            //data: {InstanceId:InsId},
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (Data) {
                Parameters[length] = Data.length;
                for (var i = 0; i < Data.length; i++) {
                    var number = i + 1;
                    var Name = Data[i].Name;
                    var UserId = "" + Data[i].UserId;
                    var Role = "" + Data[i].RoleID;
                    Parameters[number] = { Name, Role, UserId };
                }

            },
            error: function (Data) {
                alert("GetAllUser fail");
            }
        });
    }
   
    return Parameters;
}

function UpdateUserRole(RoleList, RoleID, UserID) {
    //debugger
    //alert(RoleID);
    //alert(RoleList[0]);

    var Design_ = '<option value="_value">_Name</option>';
    var Design1_ = '<option value="_value" selected>_Name</option>';
    var End_Result = '';
    for (var i = 0; i < RoleList[0] + 1; i++) {
        var local = '';

        if (RoleID == RoleList[i].Role) {
            if (RoleList[i].UserId == UserID) {
                local = Design1_.replace('_value', RoleList[i].UserId);
            }
            else {
                local = Design_.replace('_value', RoleList[i].UserId);
            }
            local = local.replace("_Name", RoleList[i].Name);
        }
        End_Result = End_Result + local;
    }
    $('#UserID').replaceWith(End_Result);
}

function PullActivityTask(PhId, InsId, PullUrl) {
        $.ajax({
        url:PullUrl ,
        data: { PhaseId: PhId, InstanceId: InsId },
        async: false,
        success: function (response) {
            if (response == false) {
                $("#PMResourceAdd").hide();
            } else {
                $("#PMResourceAdd").show();
            }
        }
    })
}

function BulkAllocationSave(BulkUrl) {
    
    var Role = $("#drpBulkRole option:selected").text();
    var RoleID = $("#drpBulkRole").val();
    var UserId = $("#drpBulkOwner").val();
    var InsId = $("#ProjInstance").val();
    var PhId = $("#drpPhase").val();

    var _RoleID = _UserId = false;
    if (RoleID == 0) {
        _RoleID = true;
        $("#lblBulkRole").html("Please select the Role").show();
    }
    else {
        $("#lblBulkRole").html("").show();
    }

    if (UserId == 0) {
        _UserId = true;
        $("#lblBulkOwner").html("please select the owner").show();
    }
    else {
        $("#lblBulkOwner").html("").show();
    }
    var a = 0;

    if (_RoleID == false && _UserId == false) {

        $.ajax({
            //type: "POST",
            url: BulkUrl ,
            async: false,
            data: { PhaseId: PhId, InstanceId: InsId, roleID: RoleID, OwnerId: UserId },
            dataType: "json",
            success: function (response) {
                //debugger;
                if (response == 'Success') {
                    //a = 1;
                    Notiflix.Notify.Success('Owner <Strong>Updated </strong>successfully..!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                }
                else {
                    Notiflix.Notify.Info('No Unassigned task to allocate for ' + Role + ' Role....!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                }
                RefreshResourceAllocation();
                RefreshProjectMonitor();
                ResetBulkAllocation();

            }
        });
    }
}

function BulkAllocationLoad(GetRoleUrl) {

    $("#addBulkAllocation").modal('show');

    var PhId = $("#drpPhase").val();
    var InsId = $("#ProjInstance").val();

    $.ajax({
        url: GetRoleUrl,
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        data: { PhaseId: PhId, InstanceId: InsId },
        success: function (data) {
            var BulkOwner = $('#drpBulkRole');
            BulkOwner.empty();
            BulkOwner.append($('<option/>', {
                value: 0,
                text: 'Select Role'
            }));
            $.each(data, function (item, value) {
                $('#drpBulkRole').append($("<option value='" + value.RoleId + "'>" + value.RoleName + "</option>"));
            })
        },
        error: function (err) {
            alert(err);
        }
    });
}

function DrpBulkAllocation(DrpUserRole) {
    var BulkRoleId = $("#drpBulkRole").val();
    var InsId = $("#ProjInstance").val();
    $.ajax({
        url: DrpUserRole,
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        data: {
            RoleID: BulkRoleId,
            InstanceId: InsId
        },
        success: function (data) {
            var BulkOwner = $('#drpBulkOwner');
            BulkOwner.empty();
            BulkOwner.append($('<option/>', {
                value: 0,
                text: 'Select Owner'
            }));
            $.each(data, function (item, value) {
                $('#drpBulkOwner').append($("<option value='" + value.UserId + "'>" + value.Name + "</option>"));
            })
        },
        error: function (err) {
            alert(err);
        }
    });
}

function DrpResourceOwnerChange(UpdateOwnerUrl,tr_Id, drp_Id) {
    
    if (drp_Id == 0) {
        drp_Id = '00000000-0000-0000-0000-000000000000';
    }
    var PhId = $("#drpPhase").val();
    var InsId = $("#ProjInstance").val();

    Notiflix.Confirm.Show(
        'Confirm',
        'Are you sure with the Owner?',
        'Yes', 'No',
        function () {
            $.ajax({
                //type: "POST",
                url: UpdateOwnerUrl,
                async: false,
                data: { PhaseId: PhId, InstanceId: InsId, rowID: tr_Id, OwnerId: drp_Id },
                //dataType: "json",
                success: function (response) {
                    if (response == 'Success') {
                        RefreshProjectMonitor();
                        UnassingnedTaskCount(PhId, InsId);
                        Notiflix.Notify.Success('Owner <Strong>Updated </strong>successfully..!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                    }
                    else {
                        Notiflix.Notify.Failure('Please Check Again..!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                    }
                },
                Failure: function () {
                    alert("In Failure")
                },
                error: function () {
                    alert("In error")
                }

            });
        }, function () {
            RefreshResourceAllocation();
        }
    );

}

function PmAddResource(MasteraddUrl) {
    $('#addResourceTask').modal('show');
    var PhId = $("#drpPhase").val();
    var InsId = $("#ProjInstance").val();
    var Pull_Task = $('#drpResTask');
    Pull_Task.empty();
    Pull_Task.append($('<option/>', {
        value: 0,
        text: 'Select Task'
    }));
    $.ajax({
        url: MasteraddUrl,
        data: { Phase_ID: PhId, InstanceId: InsId },
        dataType: "json",
        success: function (response) {
            $.each(response, function (item, value) {
                $('#drpResTask').append($("<option value='" + value.ActivityID + "'>" + value.Task + "</option>"));
            })
        }
    });
}

function ResourceCreate(ResourceCreateUrl) {

    var InsId = $("#ProjInstance").val();
    var ActivityID = $("#drpResTask").val();
    var PhId = $("#drpPhase").val();
    if (ActivityID != 0) {
        $("#lblResTask").html("").show();
        $.ajax({
            url: ResourceCreateUrl ,
            data: { InstanceId: InsId, ActivityID: ActivityID },
            dataType: "json",
            success: function (response) {
                if (response == 'Success') {
                    $('#addResourceTask').modal('hide');
                    $('.modal-backdrop').remove();
                    RefreshResourceAllocation();
                    Notiflix.Notify.Success('Task <Strong>Added </strong>successfully..!', { cssAnimationStyle: 'zoom', cssAnimationDuration: 500, });
                }

            }
        });
    }
    else {
        $("#lblResTask").html("Please select the Task").show();

    }
}



function ResetBulkAllocation() {
    $("#drpBulkRole").val(0);
    $("#drpBulkOwner").val(0);
    $("#lblBulkRole").html("").show();
    $("#lblBulkOwner").html("").show();
    $("#addBulkAllocation").modal('hide');
    $(".modal-backdrop").remove();
}
/*==================================================================Resource Allocation Transaction Page End=========================================================================== */
/*==================================================================Progress Allocation Page Start=========================================================================== */
function PulltaskTrans(PhId, InsId, UserType, PullTaskTransUrl, GetTaskCountUrl) {
    //Loading_On();
    //debugger;
    if (PhId > 0 && InsId != 0) {
        MonitorData(PhId, InsId);
        if (UserType == 'Project Manager') {
            $.ajax({
                url: PullTaskTransUrl,
                data: { PhaseId: PhId, InstanceId: InsId },
                //async: false,
                success: function (response) {
                    if (response == false) {
                        $("#PMActivityAdd").hide();
                    } else {
                        $("#PMActivityAdd").show();
                    }
                   // Loading_Off();
                }
            })
        }
        //debugger;
        Taskcount(PhId, InsId, GetTaskCountUrl);
    }
    //else {
    //}

    $('[data-toggle="tooltip"]').tooltip();
    $("#testselect").multiselect("clearSelection");
    $(".ProgressMonitor").find('tr td:hidden').show();
}

function Taskcount(PhId, InsId, GetTaskCountUrl) {
   // debugger;
    $.ajax({
        url: GetTaskCountUrl ,
        async: true,
        dataType: "json",
        data: { PhaseId: PhId, InstanceId: InsId },
        success: function (response) {
            $('.ProgressMonitor_count thead').remove();

            var tbody = '<thead class="remove_background">';
            var tr = '<tr>';
            if (response.length > 0) {
                $.each(response, function (key, val) {
                    var tr2 = '<tr style="align-text:center">';
                    bb = '<td scope="col" class="px-4">' + val.TotalTask + '</td>';
                    bb += '<td scope="col" class="px-4">' + val.Completed + '</td>';
                    bb += '<td scope="col" class="px-4">' + val.WIP + '</td>';
                    bb += '<td scope="col" class="px-4">' + val.ONHOLD + '</td>';
                    bb += '<td scope="col" class="px-4">' + val.YetToStart + '</td>';
                    bb += '<td scope="col" class="">' + val.StartDate + '</td>';
                    bb += '<td scope="col" class="">' + val.EndDate + '</td>';
                    if (val.Colour == 0) {
                        if (val.TotalTask == val.YetToStart) {
                            bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-secondary schedule" aria-hidden="true"></i>&nbsp;yet to start</small></td>';
                        }
                        else {
                            bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-secondary schedule" aria-hidden="true"></i>&nbsp;on schedule</small></td>';
                        }
                    }
                    else if (val.Colour == 1) {
                        bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-success schedule" aria-hidden="true"></i>&nbsp;completed</small></td>';
                    }
                    else if (val.Colour == 2) {
                        if (val.Completed == val.TotalTask) {
                            bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-danger schedule" aria-hidden="true"></i>&nbsp;completed</small></td>';
                        }
                        else {
                            bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-danger schedule" aria-hidden="true"></i>&nbsp;delayed</small></td>';
                        }
                    }
                    tr = tr + tr2 + bb + '</tr>' + '</tr>';
                });
            }
            else {
                var bb2 = '<td colspan="4" class="text-center">' + "<strong>No Record Found</strong>" + '</td>';
                tr = tr + bb2 + '</tr>';
            }
            tbody = tbody + tr + '</thead>';
            $('.ProgressMonitor_count').append(tbody);
        }
    });
}


function DrpOwnerUserRole(GetUserRoleUrl) {
    var RoleID = $("#drpRoles").val();
    $('#drpOwner').empty()
    $.ajax({
        url: GetUserRoleUrl ,// + "?RoleID=" + RoleID,
        //data: {RoleID:RoleID},
        data: { RoleID: RoleID, InstanceId: "00000000-0000-0000-0000-000000000000" },
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            $('#drpOwner').append($("<option value='0'>Select Owner</option>"));
            $.each(res, function (data, value) {
                var d = "00000000-0000-0000-0000-000000000000"
                if (value.UserId != d) {
                    $("#drpOwner").append($("<option></option>").val(value.UserId).html(value.Name));
                }

            })
        }
    });
}

function ParallelPhaseName(ParallelPhaseNameUrl) {
   // debugger
    //var taskValue = $(this).children("option:selected").val();
    var taskValue = $('#drpTaskType').val();
    var PhaseId = $('#drpPhase').val();
    if (taskValue == "0" || taskValue == "1" || taskValue == "3") {
        $("#parallelhideid").hide();
        $('#drpParallelId').prop('selectedIndex', 0);
        $("#drpParallelId").val("00000000-0000-0000-0000-000000000000");
    }
    else if (taskValue == "2") {
        $('#drpParallelId').prop('selectedIndex', 0);
        $("#parallelhideid").show();
        $('#drpParallelId').empty();
        var InstanceId = $('#ProjInstance').val();
        $.ajax({
            type: "GET",
            url: ParallelPhaseNameUrl,
            data: { Id: PhaseId, taskValue: taskValue, instanceid: InstanceId, first: true },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                if (response != null) {
                    $('#drpParallelId').append($("<option value='00000000-0000-0000-0000-000000000000'>New Parallel Name</option>"));
                    $.each(response, function (index, item) {
                        var option = $("<option value='" + item.ParallelId + "'>" + item.Parallel_Name + "</option>");
                        $('#drpParallelId').append(option);
                    });
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


function CreateActivityProgress(AllTaskParallelUrl, PreviousTaskUrl) {
    var Task = $("#txtTask").val();
    Task = Task.replace(/,/g, " ");
    var ApplicationArea = $("#drpApplicationArea").val();
    //var Phase = $('#drpPhase').val();
    var Owner = $('#drpOwner').val();
    var Role = $('#drpRoles').val();
    var BlockId = $('#drpBuildingBlock').val();
    var Estimated = $("#txtEstimated").val();
    var Tasktype = $("#drpTaskType").val();
    var Paralleltype = $("#drpParallelId").val();
    if (Paralleltype == null) {
        Paralleltype = '00000000-0000-0000-0000-000000000000';
    }

    var PhaseId = $("#drpPhase").val();
    var InstanceId = $('#ProjInstance').val();

    Estimated = Estimated.replace(/:/g, ".");

    var model = {
        Task: Task,
        //PhaseID: Phase,
        Owner: Owner,
        BuildingBlock_id: BlockId,
        EST_hours: Estimated,
        ApplicationAreaID: ApplicationArea,
        RoleID: Role,
        Task_id: Tasktype,
        Parallel_Id: Paralleltype
    }

    var Task_ = Owner_ = BuildingBlock_ = EST_hours_ = ApplicationAreaID_ = RoleID_ = Task_id_ = Parallel_Id = false;

    if (Task == "") {
        Task_ = true;
        $("#lblTask").html("Please enter the Task").show();
    }
    else if (TaskCheck(Task, null) == false) {
        Task_ = true;
        $("#lblTask").html("Task name is already exist").show();
    }
    else {
        $("#lblTask").html("").show();
    }

    if (ApplicationArea == 0) {
        ApplicationAreaID_ = true;
        $("#lblApplicationArea").html("Please select the Application Area").show();
    }
    else {
        $("#lblApplicationArea").html("").show();
    }

    if (Owner == 0) {
        Owner_ = true;
        $("#lblOwner").html("Please select the Owner").show();
    }
    else {
        $("#lblOwner").html("").show();
    }

    if (Role == 0) {
        RoleID_ = true;
        $("#lblRoles").html("Please select the Role").show();
    }
    else {
        $("#lblRoles").html("").show();
    }

    if (BlockId == 0) {
        BuildingBlock_ = true;
        $("#lblBuildingBlock").html("Please select the Building Block").show();
    }
    else {
        $("#lblBuildingBlock").html("").show();
    }

    if (Estimated == "") {
        EST_hours_ = true;
        $("#lblEstimated").html("Please enter Est hours").show();
    }
    else if (TaskEstHrs(Estimated) == false) {
        EST_hours_ = true;
        $("#lblEstimated").html("Invalid Est hrs").show();
    }
    else {
        $("#lblEstimated").html("").show();
    }
    if (Tasktype == 0) {
        Task_id_ = true;
        $("#lblTaskType").html("Please select the Task Type").show();
    }
    else {
        $("#lblTaskType").html("").show();
    }

    if (Task_ == false && Owner_ == false && BuildingBlock_ == false && EST_hours_ == false && ApplicationAreaID_ == false && RoleID_ == false && Task_id_ == false && Parallel_Id == false) {
        $("#drpTsk").empty();
        var id = $("#drpPhase").val()
        //$('#Prev-Activity').modal('show');
        $('#addActivityTask').modal('hide');
        $('#Prev-Activity').modal('show');

        if (id > 0 && (Paralleltype != '00000000-0000-0000-0000-000000000000')) {
            $("#newParallelrow").hide();
            var VID ='00000000-0000-0000-0000-000000000000'
            $.ajax({
                type: "GET",
                url: AllTaskParallelUrl,
                data: { id: id, Parallel_Id: Paralleltype, instanceid: InstanceId, first: true, VID: VID },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        // $('#drpTsk').append($("<option value='0'>Please Select Previous Task</option>"));
                        $.each(response, function (index, item) {
                            var option = $("<option value='" + item.Activity_ID + "'>" + item.Task + "</option>");
                            $('#drpTsk').append(option);
                        });
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
        else if (id > 0) {
            $("#newParallelrow").hide();
            if (Tasktype == 2 && Paralleltype == '00000000-0000-0000-0000-000000000000') {
                $("#newParallelrow").show();
            }
            $.ajax({
                url: PreviousTaskUrl,
                data: { PhaseId: PhaseId, InstanceId: InstanceId },
                dataType: "json",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    if (response != null) {
                        $('#drpTsk').append($("<option value='0'>Please Select Previous Task</option>"));
                        $.each(response, function (index, item) {
                            var option = $("<option value='" + item.Activity_ID + "'>" + item.Task + "</option>");
                            $('#drpTsk').append(option);
                        });
                    }
                },
                error: function (data) {
                    alert("GetPreviousTaskID phaseid fail");
                }
            });
        }
    }
}

/*==================================================================Progress Allocation Page End=========================================================================== */
