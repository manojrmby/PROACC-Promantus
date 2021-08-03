//Toogle
//function Dash_Toggle() {  
//    var x = document.getElementById("idDashCard");
//    var y = document.getElementById("idDashPrograssBar");
//    if (x.style.display === "none") {
//        x.style.display = "block";
//        y.style.display = "none";
//    } else {
//        y.style.display = "block";
//        x.style.display = "none";
//    }
//}

// Dashboard Build
function Build_DontCard(cc) {
    var Ass = '', Pre = '', Con = '', Post = '';
    var Temp = '';

    Temp = '<div class="card rounded-0 border-0 shadow my-3">';
    Temp = Temp + '<div class="card-body">';
    Temp = Temp + '<div class="row d-flex justify-content-between align-items-left px-2"><span class="card-title mb-0">Instance: InstanceName</span>';//InstanceName
    Temp = Temp + '<div class="progress-sch"><span class="p-0 time-sch border-0"> _CurrentStatus&nbsp;&nbsp;<i class="fa fa-circle schedule _CircleClass" aria-hidden="true"></i></span>';//Schedule
    Temp = Temp + '</div>';
    Temp = Temp + '<span class="card-text mb-2 d-block w-100 color-gray font-10">Project: project_Name</span>';//Project Name
    Temp = Temp + '</div>';
    Temp = Temp + '<div class="row px-2 d-flex justify-content-between align-items-left">';
    Temp = Temp + '<div class="col-md-10">';
    Temp = Temp + '<div class="row d-flex justify-content-between align-items-left">';
    //Temp = Temp + '<div class=""><canvas id=Chart_id></canvas></div>';//Chart

    //Temp = Temp + '<div style="width:65%;"><canvas class="chart" id=Chart_id></canvas></div>';//Chart
    Temp = Temp + '<div style="width:65%;"> <div id="Chart_id" style="max-height:80px;max-width:130px;"></div></div>';//Chart

    Temp = Temp + '<div class=""><span class="card-text mb-3 d-block w-100">Task done <br><span class="semibold font-14">Comp_Task</span>/Total_Task</span>';//TOTAL-Comp
    Temp = Temp + '</div>';
    Temp = Temp + '</div>';

    Temp = Temp + '<div class="row px-0 color-gray">';
    Temp = Temp + '<span class="card-text mb-0 d-block w-100 font-10"><i class="fa fa-calendar" aria-hidden="true"></i> &nbsp;StartDate - EndDate</span>';//Date
    Temp = Temp + '<span class="p-0 time-sch w-100 font-10"><span class="float-left">Client_Name</span><span class="float-right"><i class="fa fa-check-circle _ReadinessCheck"></i>&nbsp;&nbsp;Readiness check</span></span>';//client name
    Temp = Temp + '</div>';
    Temp = Temp + '</div>';
    Temp = Temp + '<a>';
    //Temp = Temp + '<div class="col-md-4 px-0"><ul class="project-acme p-0 m-0 text-right">';//Profile
    //Temp = Temp + '<li class="float-none"><a class="profileImage"></a></li>';
    //Temp = Temp + '<li class="float-none"><a class="profileImage"></a></li>';
    //Temp = Temp + '<li class="float-none"><a class="profileImage"></a></li>';
    //Temp = Temp + '<li class="float-none"><a class="profileImage"></a></li>';
    //Temp = Temp + ' </ul></div>';



    Temp = Temp + '</div>';
    Temp = Temp + '</div>';
    Temp = Temp + '</div>';

    var Ass_Prog = '', Pre_Prog = '', Con_Prog = '', Post_Prog = '';
    var Temp_Prog = '';
    Temp_Prog = Temp_Prog + '<tr>';

    Temp_Prog = Temp_Prog + '<div class="flag d-none">';
    Temp_Prog = Temp_Prog + '<a href="javascript:;"><i class="fa fa-font-awesome font-16" aria-hidden="true"></i></a>';
    Temp_Prog = Temp_Prog + '</div>';
    Temp_Prog = Temp_Prog + '<td><i class="fa fa-th" aria-hidden="true"></i></td>';
    Temp_Prog = Temp_Prog + '<td>';
    Temp_Prog = Temp_Prog + '<span>InstanceName</span>';//instance
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '<td>';
    Temp_Prog = Temp_Prog + '<span class="card-text mb-0 d-block w-100 color-gray font-12">project_Name</span>';//Project
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '<td class="">';
    Temp_Prog = Temp_Prog + '<div class="form-row">';
    Temp_Prog = Temp_Prog + '<span class="task-done">';
    Temp_Prog = Temp_Prog + '<span class="semibold font-14">Comp_Task</span>/Total_Task';//total and done
    Temp_Prog = Temp_Prog + '</span>';
    Temp_Prog = Temp_Prog + '<div class="progress mb-1">';
    Temp_Prog = Temp_Prog + '<div class="progress-bar" style="width:per_cent" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100"></div>'; //width value
    Temp_Prog = Temp_Prog + '</div>';
    Temp_Prog = Temp_Prog + '</div>';
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '<td>';
    Temp_Prog = Temp_Prog + '<span class="card-text mb-0 d-block w-100 font-12 color-gray"><i class="fa fa-calendar" aria-hidden="true"></i> &nbsp;StartDate - EndDate</span>';//Date
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '<td>';
    Temp_Prog = Temp_Prog + '<span class="card-text mb-0 d-block w-100 font-12 color-gray"><i class="fa fa-check-circle _ReadinessCheck"></i></span>';//Readiness Check
    Temp_Prog = Temp_Prog + '</td>';

    Temp_Prog = Temp_Prog + '<a>';
    //Temp_Prog = Temp_Prog + '<td class="float-right">';//profile
    //    Temp_Prog = Temp_Prog + '<ul class="project-acme p-0 m-0 text-right">';
    //        Temp_Prog = Temp_Prog + '<li class="">';
    //        Temp_Prog = Temp_Prog + '<img src="../assets/img/consultants/dp2.png">';
    //        Temp_Prog = Temp_Prog + '</li>';
    //        Temp_Prog = Temp_Prog + '<li class="">';
    //        Temp_Prog = Temp_Prog + '<img src="../assets/img/consultants/dp4.png">';
    //        Temp_Prog = Temp_Prog + '</li>';
    //        Temp_Prog = Temp_Prog + '<li class="">';
    //        Temp_Prog = Temp_Prog + '<img src="../assets/img/consultants/dp5.png">';
    //        Temp_Prog = Temp_Prog + '</li>';
    //        Temp_Prog = Temp_Prog + '<li class="">';
    //        Temp_Prog = Temp_Prog + '<div class="card-plus">';
    //        //Temp_Prog = '<i class="fa fa-plus" aria-hidden="true"></i>';
    //        Temp_Prog = Temp_Prog + '</div>';
    //        Temp_Prog = Temp_Prog + '</li>';
    //    Temp_Prog = Temp_Prog + '</ul>';
    //Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '<td>';
    Temp_Prog = Temp_Prog + '<span class="p-0 time-sch border-0"> _CurrentStatus&nbsp;&nbsp;<i class="fa fa-circle schedule _CircleClass" aria-hidden="true"></i></span>';
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '<td>';
    Temp_Prog = Temp_Prog + '<span class="card-text mb-2 d-block w-100 color-gray font-12">Client_Name</span>';//client
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '<td>';
    Temp_Prog = Temp_Prog + '<span class="card-text mb-2 d-block w-100 color-gray font-12"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></span>';
    Temp_Prog = Temp_Prog + '</td>';
    Temp_Prog = Temp_Prog + '</tr>';



    var Local = '', local1 = '', Per = '';
    
    for (var i = 0; i < cc.length; i++) {
        {
            //if (cc[i].Total_Task != cc[i].Comp_Task) {
                if (cc[i].PhaseId == 1) {// Ass
                    Local = '';
                    Local = Temp;
                    per_cent = '';
                    local1 = '';
                    local1 = Temp_Prog;
                    if (cc[i].ReadinessCheck == 1) {
                        Local = Local.replace("_ReadinessCheck", 'icon-green');
                        local1 = local1.replace("_ReadinessCheck", 'icon-green');
                    }





                    Local = Local.replace("project_Name", cc[i].ProjectName);
                    Local = Local.replace("Total_Task", cc[i].Total_Task);
                    Local = Local.replace("Comp_Task", cc[i].Comp_Task);
                    Local = Local.replace("InstanceName", cc[i].InstanceName);

                    Local = Local.replace("<a>", Build_letter(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    Local = Local.replace("Chart_id", cc[i].InstanceId + '_' + cc[i].PhaseId);
                    Local = Local.replace("Client_Name", cc[i].customerName);
                    Local = Local.replace("StartDate", cc[i].StartDate);
                    Local = Local.replace("EndDate", cc[i].EndDate);
                    Local=Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart,cc[i].Colour, Local);
                    Ass = Ass + Local;



                    
                    per_cent = Math.round(((cc[i].Comp_Task / cc[i].Total_Task) * 100)) + "%";
                    local1 = local1.replace("project_Name", cc[i].ProjectName);
                    local1 = local1.replace("Total_Task", cc[i].Total_Task);
                    local1 = local1.replace("Comp_Task", cc[i].Comp_Task);
                    local1 = local1.replace("InstanceName", cc[i].InstanceName);
                    local1 = local1.replace("<a>", Build_letter_Prograss(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    local1 = local1.replace("Client_Name", cc[i].customerName);
                    local1 = local1.replace("StartDate", cc[i].StartDate);
                    local1 = local1.replace("EndDate", cc[i].EndDate);
                    local1 = local1.replace("per_cent", per_cent);
                    local1 = Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart, cc[i].Colour, local1);
                    Ass_Prog = Ass_Prog + local1;


                }
                else if (cc[i].PhaseId == 2) {//Pre-
                    Local = '';
                    Local = Temp;
                    per_cent = '';
                    local1 = '';
                    local1 = Temp_Prog;
                    var remove = '<span class="float-right"><i class="fa fa-check-circle _ReadinessCheck"></i>&nbsp;&nbsp;Readiness check</span>';
                    Local = Local.replace(remove, "");
                    var remove1 = '<td><span class="card-text mb-0 d-block w-100 font-12 color-gray"><i class="fa fa-check-circle _ReadinessCheck"></i></span></td>';
                    local1 = local1.replace(remove1, "");
                    //if (cc[i].ReadinessCheck == 1) {
                    //    Local = Local.replace("_ReadinessCheck", 'icon-green');
                    //    local1 = local1.replace("_ReadinessCheck", 'icon-green');
                    //}


                    Local = Local.replace("project_Name", cc[i].ProjectName);
                    Local = Local.replace("Total_Task", cc[i].Total_Task);
                    Local = Local.replace("Comp_Task", cc[i].Comp_Task);
                    Local = Local.replace("InstanceName", cc[i].InstanceName);
                    Local = Local.replace("<a>", Build_letter(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    Local = Local.replace("Chart_id", cc[i].InstanceId + '_' + cc[i].PhaseId);
                    Local = Local.replace("Client_Name", cc[i].customerName);
                    Local = Local.replace("StartDate", cc[i].StartDate);
                    Local = Local.replace("EndDate", cc[i].EndDate);
                    Local = Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart, cc[i].Colour, Local);
                    Pre = Pre + Local;

                   
                    per_cent = Math.round(((cc[i].Comp_Task / cc[i].Total_Task) * 100)) + "%";
                    local1 = local1.replace("project_Name", cc[i].ProjectName);
                    local1 = local1.replace("Total_Task", cc[i].Total_Task);
                    local1 = local1.replace("Comp_Task", cc[i].Comp_Task);
                    local1 = local1.replace("InstanceName", cc[i].InstanceName);
                    local1 = local1.replace("<a>", Build_letter_Prograss(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    local1 = local1.replace("Client_Name", cc[i].customerName);
                    local1 = local1.replace("StartDate", cc[i].StartDate);
                    local1 = local1.replace("EndDate", cc[i].EndDate);
                    local1 = local1.replace("per_cent", per_cent);
                    local1 = Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart, cc[i].Colour, local1);
                    Pre_Prog = Pre_Prog + local1;

                }
                else if (cc[i].PhaseId == 3) {//Con
                    Local = '';
                    Local = Temp;
                    per_cent = '';
                    local1 = '';
                    local1 = Temp_Prog;
                    var remove = '<span class="float-right"><i class="fa fa-check-circle _ReadinessCheck"></i>&nbsp;&nbsp;Readiness check</span>';
                    Local = Local.replace(remove, "");
                    var remove1 = '<td><span class="card-text mb-0 d-block w-100 font-12 color-gray"><i class="fa fa-check-circle _ReadinessCheck"></i></span></td>';
                    local1 = local1.replace(remove1, "");
                    //if (cc[i].ReadinessCheck == 1) {
                    //    Local = Local.replace("_ReadinessCheck", 'icon-green');
                    //    local1 = local1.replace("_ReadinessCheck", 'icon-green');
                    //}


                    Local = Local.replace("project_Name", cc[i].ProjectName);
                    Local = Local.replace("Total_Task", cc[i].Total_Task);
                    Local = Local.replace("Comp_Task", cc[i].Comp_Task);
                    Local = Local.replace("InstanceName", cc[i].InstanceName);
                    Local = Local.replace("<a>", Build_letter(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    Local = Local.replace("Chart_id", cc[i].InstanceId + '_' + cc[i].PhaseId);
                    Local = Local.replace("Client_Name", cc[i].customerName);
                    Local = Local.replace("StartDate", cc[i].StartDate);
                    Local = Local.replace("EndDate", cc[i].EndDate);
                    Local = Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart, cc[i].Colour, Local);
                    Con = Con + Local;

                   
                    per_cent = Math.round(((cc[i].Comp_Task / cc[i].Total_Task) * 100)) + "%";
                    local1 = local1.replace("project_Name", cc[i].ProjectName);
                    local1 = local1.replace("Total_Task", cc[i].Total_Task);
                    local1 = local1.replace("Comp_Task", cc[i].Comp_Task);
                    local1 = local1.replace("InstanceName", cc[i].InstanceName);
                    local1 = local1.replace("<a>", Build_letter_Prograss(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    local1 = local1.replace("Client_Name", cc[i].customerName);
                    local1 = local1.replace("StartDate", cc[i].StartDate);
                    local1 = local1.replace("EndDate", cc[i].EndDate);
                    local1 = local1.replace("per_cent", per_cent);
                    local1 = Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart, cc[i].Colour, local1);
                    Con_Prog = Con_Prog + local1;

                }
                else if (cc[i].PhaseId == 4) {//Post 
                    Local = '';
                    Local = Temp;

                    per_cent = '';
                    local1 = '';
                    local1 = Temp_Prog;
                    var remove = '<span class="float-right"><i class="fa fa-check-circle _ReadinessCheck"></i>&nbsp;&nbsp;Readiness check</span>';
                    Local = Local.replace(remove, "");
                    var remove1 = '<td><span class="card-text mb-0 d-block w-100 font-12 color-gray"><i class="fa fa-check-circle _ReadinessCheck"></i></span></td>';
                    local1 = local1.replace(remove1, "");

                    //if (cc[i].ReadinessCheck == 1) {
                    //    Local = Local.replace("_ReadinessCheck", 'icon-green');
                    //    local1 = local1.replace("_ReadinessCheck", 'icon-green');
                    //}

                    Local = Local.replace("project_Name", cc[i].ProjectName);
                    Local = Local.replace("Total_Task", cc[i].Total_Task);
                    Local = Local.replace("Comp_Task", cc[i].Comp_Task);
                    Local = Local.replace("InstanceName", cc[i].InstanceName);
                    Local = Local.replace("<a>", Build_letter(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    Local = Local.replace("Chart_id", cc[i].InstanceId + '_' + cc[i].PhaseId);
                    Local = Local.replace("Client_Name", cc[i].customerName);
                    Local = Local.replace("StartDate", cc[i].StartDate);
                    Local = Local.replace("EndDate", cc[i].EndDate);
                    Local = Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart, cc[i].Colour, Local);
                    Post = Post + Local;

                   
                    per_cent = Math.round(((cc[i].Comp_Task / cc[i].Total_Task) * 100)) + "%";
                    local1 = local1.replace("project_Name", cc[i].ProjectName);
                    local1 = local1.replace("Total_Task", cc[i].Total_Task);
                    local1 = local1.replace("Comp_Task", cc[i].Comp_Task);
                    local1 = local1.replace("InstanceName", cc[i].InstanceName);
                    local1 = local1.replace("<a>", Build_letter_Prograss(cc[i].Top5Con_ID, cc[i].Top5Con_Name, cc[i].Top5Con_Image));
                    local1 = local1.replace("Client_Name", cc[i].customerName);
                    local1 = local1.replace("StartDate", cc[i].StartDate);
                    local1 = local1.replace("EndDate", cc[i].EndDate);
                    local1 = local1.replace("per_cent", per_cent);
                    local1 = Card_status(cc[i].TotalTask, cc[i].Completed, cc[i].YetToStart, cc[i].Colour, local1);
                    Post_Prog = Post_Prog + local1;
                }
           // }
            //Client_Name
            
        }
    }


    $('#Assment_Cards').replaceWith(Ass);
    $('#Pre-Con_Cards').replaceWith(Pre);
    $('#Con_Cards').replaceWith(Con);
    $('#Post-Con_Cards').replaceWith(Post);

    //debugger;
    $('#Assment_Prograss tr').replaceWith(Ass_Prog);
    $('#Pre_Prograss tr').replaceWith(Pre_Prog);

    $('#Conv_Prograss tr').replaceWith(Con_Prog);
    $('#Post_Prograss tr').replaceWith(Post_Prog);
}
function Build_letter(IDs, Names, Con_Image) {
    //debugger;
    var li_s = '<div class="col-md-2 px-0"> <ul class="project-acme p-0 m-0 text-right">';//Profile
    //li_s = li_s + '<li class="float-none"><a class="profileImage" id=1>1_</a></li>';
    //li_s = li_s + '<li class="float-none"><a class="profileImage" id=2>2_</a></li>';
    //li_s = li_s + '<li class="float-none"><a class="profileImage"id=3>3_</a></li>';
    //li_s = li_s + '<li class="float-none"><a class="profileImage"id=44>44_</a></li>';
    li_s = li_s + '<a>'
    li_s = li_s + ' </ul></div >';

    var ID_Set = SplitByComaa(IDs);
    var Name_Set = SplitByComaa(Names);
    var Image_Set = SplitByComaa(Con_Image);
    var final = '';
    var End_Result = '';
    
    if (ID_Set != "") {
        for (var i = 0; i < ID_Set.length; i++) {

            var IN_Res_ = '<li class="float-none">';
            var ImageId = Image_Set[i];
            if (ImageId != undefined) {
                if (ImageId.toString().trim() != "") {
                    IN_Res_ = IN_Res_ + '<a class="" data-toggle="tooltip" title="_title" id=_id><img src="../assets/PhotoUpload/' + ImageId.toString().trim() + '.jpg" class="profileImage" style="padding:0px !important"></a>';

                }
                else {
                    IN_Res_ = IN_Res_ + '<a class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</a>';
                }
            }
            else {
                IN_Res_ = IN_Res_ + '<a class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</a>';
            }
            var local = '';
            IN_Res_ = IN_Res_ + '</li>';

            local = IN_Res_.replace('_id', ID_Set[i]);
            local = local.replace("_cla", getSColor(random(1, 7)));
            local = local.replace('_Data', GetTwoLetters(Name_Set[i].trim()));
            local = local.replace('_title', Name_Set[i].trim());
            End_Result = End_Result + local;
        }
        final = li_s.replace('<a>', End_Result);
    }
    else {
        final = li_s.replace('<a>', "");
    }

    return final;
}
function Build_letter_Prograss(IDs, Names, Con_Image) {
    var td = '';
    td = td + '<td>';//profile
    td = td + '<ul class="project-acme p-0 m-0 text-right">';
    td = td + '<a>'
    //td = td + '<li class="">';
    //td = td + '<img src="../assets/img/consultants/dp2.png">';
    //td = td + '</li>';
    //td = td + '<li class="">';
    //td = td + '<img src="../assets/img/consultants/dp4.png">';
    //td = td + '</li>';
    //td = td + '<li class="">';
    //td = td + '<img src="../assets/img/consultants/dp5.png">';
    //td = td + '</li>';
    //td = td + '<li class="">';
    //td = td + '<div class="card-plus">';
    ////td = '<i class="fa fa-plus" aria-hidden="true"></i>';
    //td = td + '</div>';
    //td = td + '</li>';
    td = td + '</ul>';
    td = td + '</td>';

    //var td_Res_ = '<li class=""><img src="../assets/img/consultants/dp2.png"></li>';
   
    var ID_Set = SplitByComaa(IDs);
    var Name_Set = SplitByComaa(Names);
    var Image_Set = SplitByComaa(Con_Image);
    var final = '';
    var End_Result = '';
    if (ID_Set != "") {
        for (var i = 0; i < ID_Set.length; i++) {

            var td_Res_ = '<li class="">';
            
           
            var ImageId = Image_Set[i];
            if (ImageId != undefined) {
                if (ImageId.toString().trim() != "") {
                    td_Res_ = td_Res_ + '<a class="" data-toggle="tooltip" title="_title" id=' + ImageId + '><img src="../assets/PhotoUpload/' + ImageId.toString().trim() + '.jpg" class="profileImage" style="padding:0px !important"></a>';
                }
                else {
                    td_Res_ = td_Res_ + '<a class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</a>';
                }
            }
            else {
                td_Res_ = td_Res_ + '<a class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</a>';
            }
            td_Res_ = td_Res_ + '</li>';
            var local = '';
            local = td_Res_.replace('_id', ID_Set[i]);
            local = local.replace("_cla", getSColor(random(1, 7)));
            local = local.replace('_Data', GetTwoLetters(Name_Set[i].trim()));
            local = local.replace('_title', Name_Set[i].trim());
            End_Result = End_Result + local;
        }
        final = td.replace('<a>', End_Result);
    }
    else {
        final = td.replace('<a>', "");
    }

    //debugger;
    return final;

}
function SplitByComaa(Dat) {
    var strArr = Dat.split(',');
    return strArr;
}
function GetTwoLetters(Name) {
    var res = Name.split(" ");
    var firstName = res[0];
    var lastName = '';
    var intials = '';
    if (res.length > 1) {
        lastName = res[1];
    }
    var intials = (firstName.charAt(0) + lastName.charAt(0)).toUpperCase();
    //var profileImage = $('#' + ID).text(intials);
    return intials;
}
function Card_status(TotalTask, Completed, YetToStart, colur, local) {
    if (colur == 0) {
        local = local.replace("_CircleClass", "text-secondary");
        if (TotalTask == YetToStart) {
            local = local.replace("_CurrentStatus", "Yet to start");
            
           // bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-secondary schedule" aria-hidden="true"></i>&nbsp;yet to start</small></td>';
        }
        else {
            local = local.replace("_CurrentStatus", "On schedule");
           // bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-secondary schedule" aria-hidden="true"></i>&nbsp;on schedule</small></td>';
        }
    }
    else if (colur == 1) {
        local = local.replace("_CurrentStatus", "Completed");
        local = local.replace("_CircleClass", "text-success");
       // bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-success schedule" aria-hidden="true"></i>&nbsp;completed</small></td>';
    }
    else if (colur == 2) {
        local = local.replace("_CircleClass", "text-danger");
        if (Completed == TotalTask) {
            local = local.replace("_CurrentStatus", "Completed");
           // bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-danger schedule" aria-hidden="true"></i>&nbsp;completed</small></td>';
        }
        else {
            local = local.replace("_CurrentStatus", "Delayed");
            //bb += '<td scope="col" class=""><small class="scheduler"><i class="fa fa-circle text-danger schedule" aria-hidden="true"></i>&nbsp;delayed</small></td>';
        }
    }
    return local;

}
//DashBoardTop
function Top_Profile(Client_Top_Ids, Client_Top_Image, Client_Top_Names, Consu_Top_Ids, Consu_Top_Image, Consu_Top_Names) {

    
    var Client_Ids = SplitByComaa(Client_Top_Ids);
    var Client_Ids_Image = SplitByComaa(Client_Top_Image);
    var Client_Names = SplitByComaa(Client_Top_Names);

    var Consu_Ids = SplitByComaa(Consu_Top_Ids);
    var Consu_Ids_Image = SplitByComaa(Consu_Top_Image);
    var Consu_Names = SplitByComaa(Consu_Top_Names);


    var Client_Design = Build_Top_Profile(Client_Ids, Client_Names, Client_Ids_Image);
    var Consu_Design = Build_Top_Profile(Consu_Ids, Consu_Names, Consu_Ids_Image);

    $('#ClientProfile_Top').append(Client_Design);
    $('#ConsultProfile_Top').append(Consu_Design);

}
function Build_Top_Profile(ID_Set, Name_Set, Ids_Image) {
    //var Design_ = '<li class=""><img src="../assets/img/consultants/dp2.png"></li>';
    // var Design_ = '<a href="javascript:;">';
    // Design_ = Design_ + '<span class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</span>';
    //// Design_ = Design_ + '<img src="../assets/img/consultants/dp2.png">';
    // Design_ = Design_ + '</a>';
    var End_Result = '';
    for (var i = 0; i < ID_Set.length; i++) {

        var Design_ = '<a href="javascript:;">';
        var ImageId = Ids_Image[i];
        if (ImageId != undefined) {
            if (ImageId.toString().trim() != "") {
                Design_ = Design_ + '<span class="" data-toggle="tooltip" title="_title" id='+ImageId.toString().trim()+'><img src="../assets/PhotoUpload/' + ImageId.toString().trim() + '.jpg" class="profileImage" style="padding:0px !important"></span>';
            } else {
                Design_ = Design_ + '<span class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</span>';
            }
        }
        else {
            Design_ = Design_ + '<span class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</span>';
        }
        //if (Ids_Image[i].toString().trim() != "" || Ids_Image[i] != ) {
        //   // Design_ = Design_ + '<img src="../assets/img/consultants/dp2.png">';

        //}
        //else {
        //    Design_ = Design_ + '<span class="profileImage _cla" data-toggle="tooltip" title="_title" id=_id>_Data</span>';
        //}

        // Design_ = Design_ + '<img src="../assets/img/consultants/dp2.png">';
        Design_ = Design_ + '</a>';


        var local = '';
        local = Design_.replace('_id', ID_Set[i]);
        local = local.replace("_cla", getSColor(random(1, 7)));
        local = local.replace('_Data', GetTwoLetters(Name_Set[i].trim()));
        local = local.replace('_title', Name_Set[i].trim());
        End_Result = End_Result + local;
    }
    return End_Result
}



//write text plugin
Chart.pluginService.register({
    afterUpdate: function (chart) {
        if (chart.config.options.elements.center) {
            var helpers = Chart.helpers;
            var centerConfig = chart.config.options.elements.center;
            var globalConfig = Chart.defaults.global;
            var ctx = chart.chart.ctx;

            var fontStyle = helpers.getValueOrDefault(centerConfig.fontStyle, globalConfig.defaultFontStyle);
            var fontFamily = helpers.getValueOrDefault(centerConfig.fontFamily, globalConfig.defaultFontFamily);

            if (centerConfig.fontSize)
                var fontSize = centerConfig.fontSize;
            // figure out the best font size, if one is not specified
            else {
                ctx.save();
                var fontSize = helpers.getValueOrDefault(centerConfig.minFontSize, 1);
                var maxFontSize = helpers.getValueOrDefault(centerConfig.maxFontSize, 256);
                var maxText = helpers.getValueOrDefault(centerConfig.maxText, centerConfig.text);

                do {
                    ctx.font = helpers.fontString(fontSize, fontStyle, fontFamily);
                    var textWidth = ctx.measureText(maxText).width;

                    // check if it fits, is within configured limits and that we are not simply toggling back and forth
                    if (textWidth < chart.innerRadius * 2 && fontSize < maxFontSize)
                        fontSize += 1;
                    else {
                        // reverse last step
                        fontSize -= 1;
                        break;
                    }
                } while (true)
                ctx.restore();
            }

            // save properties
            chart.center = {
                font: helpers.fontString(fontSize, fontStyle, fontFamily),
                fillStyle: helpers.getValueOrDefault(centerConfig.fontColor, globalConfig.defaultFontColor)
            };
        }
    },
    beforeDraw: function (chart) {
        if (chart.center) {
            var centerConfig = chart.config.options.elements.center;
            var ctx = chart.chart.ctx;

            ctx.save();
            ctx.font = chart.center.font;
            ctx.fillStyle = chart.center.fillStyle;
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
            var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;
            ctx.fillText(centerConfig.text, centerX, centerY);
            ctx.restore();
        }
    },
})

function Chat_Top_Fun(Total_Task, Total_Task_Comp) {

    Chart_S('myChart', Total_Task, Total_Task_Comp);
   
}
function Build_Dynamic_Charts(cc) {
    //debugger
    var id = '', Tot = '', Comp = '';
    for (var i = 0; i < cc.length; i++) {
        //if (cc[i].Total_Task != cc[i].Comp_Task) {
            id = ''; Tot = ''; Comp = '';

            id = cc[i].InstanceId + '_' + cc[i].PhaseId;
            Tot = cc[i].Total_Task;
            Comp = cc[i].Comp_Task;

        //Chart_S(id, Tot, Comp);
        TotalvsCompleted(id,Tot, Comp);
        //}
    }
}
function Chart_S(ID, Total_Task, Total_Task_Comp) {

    var percentage = '';
    percentage = Math.round(((Total_Task_Comp / Total_Task) * 100)) + "%";
    //debugger;
    if (Total_Task_Comp === Total_Task) {
        Total_Task = 0;
    }
    //Total_Task_Comp = 70;
    var Chart_Top = {
        type: 'doughnut',
        data: {
            labels: [
                "Total",
                "Completed"
            ],
            datasets: [{
                data: [Total_Task, Total_Task_Comp],
                backgroundColor: [

                    "#c1e5f4",
                    "#0084C9",
                ],
                hoverBackgroundColor: [

                    "#c1e5f4",
                    "#0084C9",
                ]
            }]
        },
        options: {
            elements: {
                arc: {
                    roundedCornersFor: 0
                },
                center: {
                    // the longest text that could appear in the center
                    maxText: '100%',
                    text: percentage,
                    fontColor: '#0084C9',
                    //fontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                    fontStyle: 'auto',
                    // fontSize: 12,
                    // if a fontSize is NOT specified, we will scale (within the below limits) maxText to take up the maximum space in the center
                    // if these are not specified either, we default to 1 and 256
                    minFontSize: 1,
                    maxFontSize: 256,
                }
            },
            legend: {
                display: false,
                labels: {
                    fontColor: 'rgb(255, 99, 132)'
                }
            },

            tooltips: {
                //callbacks: {
                //    title: function (tooltipItem, data) {
                //        return data['labels'][tooltipItem[0]['index']];
                //    },
                //    label: function (tooltipItem, data) {
                //        return data['datasets'][0]['data'][tooltipItem['index']];
                //    },
                //    afterLabel: function (tooltipItem, data) {
                //        var dataset = data['datasets'][0];
                //        var percent = Math.round((dataset['data'][tooltipItem['index']] / dataset["_meta"][0]['total']) * 100)
                //        return '(' + percent + '%)';
                //    },

                //},
                //backgroundColor: '#FFF',
                titleFontSize: 2,
                //titleFontColor: '#0066ff',
                //bodyFontColor: '#000',
                bodyFontSize: 10,
                displayColors: false
            }
        }
    };
    var ctx = document.getElementById(ID).getContext("2d");
    var myChart = new Chart(ctx, Chart_Top);
}

function TotalvsCompleted(ID,Total_Task, Total_Task_Comp) {

    // Create chart instance
    var chart = am4core.create(ID, am4charts.PieChart);
    var Remaining_Task = Total_Task - Total_Task_Comp;
    data_ = [Total_Task_Comp, Remaining_Task];
    labels = ["Completed", "Remaining"];
    let Data_ = ConvertToJSON(data_, labels);

    var percentage = '';
    percentage = Math.round(((Total_Task_Comp / Total_Task) * 100)) + "%";

    chart.data = Data_;
    chart.innerRadius = am4core.percent(10);
    // Add and configure Series
    var pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "count";
    pieSeries.dataFields.category = "Name";
    pieSeries.innerRadius = am4core.percent(50);
    pieSeries.ticks.template.disabled = true;
    pieSeries.labels.template.disabled = true;

    var rgm = new am4core.RadialGradientModifier();
    rgm.brightnesses.push(-0.8, -0.8, -0.5, 0, - 0.5);
    pieSeries.slices.template.fillModifier = rgm;
    pieSeries.slices.template.strokeModifier = rgm;
    pieSeries.slices.template.strokeOpacity = 0.4;
    pieSeries.slices.template.strokeWidth = 0;

    var label = pieSeries.createChild(am4core.Label);
    label.text = percentage;
    label.horizontalCenter = "middle";
    label.verticalCenter = "middle";
    label.fontSize = 10;

    pieSeries.colors.list = [
        //am4core.color("#845EC2"),
        //am4core.color("#D65DB1"),
        //am4core.color("#FF6F91"),
        //am4core.color("#FF9671"),

        //am4core.color("#FFC75F"),
        //am4core.color("#F9F871"),
        am4core.color("#0084C9"),
        am4core.color("#c1e5f4")        
    ];
}

function ConvertToJSON(Data, Name) {
    const mapArrays = (Name, Data) => {
        const res = [];
        for (let i = 0; i < Name.length; i++) {
            res.push({
                Name: Name[i],
                count: Data[i]
            });
        };
        return res;
    };
    var result = mapArrays(Name, Data);
    return result;
} 