
$(document).ready(function () {
    $(window).on("load", function () {
        $(".loader-wrapper").fadeOut("slow");
    });

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    $(".onclickToastr").on("click", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        var email = $(this).prev().val();
        //console.log(url + "" + "?search=" + email);
        var getUrl = url + `?email=` + email+"";
        fetch(getUrl)
            .then(response => response.json())
            .then(data => {
                if (data.isSucceeded == false) {
                    toastr.error(""+data.message)
                }
                else if (data.isSucceeded == true) {
                    toastr.success(""+data.message)
                }
            });
    })




    $(document).on("click", ".delete-profile-btn", function (e) {

        e.preventDefault();

        var url = $(this).attr("href")

        Swal.fire({
            title: 'Hesabınızı silmək istədiyinizdən əminsinizmi?',
            text: "Geri qaytara bilməyəcəksiz",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Bəli, silinsin!',
            cancelButtonText: "Xeyr"
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => response.json())
                    .then(data => console.log(data));
                location.reload(true);
                Swal.fire(
                    'Silindi!',
                    'Sizin hesabınız silindi.',
                    'success'
                )
            }


        })
    })
    $(document).on("click", ".delete-job-btn", function (e) {

        e.preventDefault();

        var url = $(this).attr("href")

        Swal.fire({
            title: 'Əminsinizmi?',
            text: "Geri qaytara bilməyəcəksiz",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Bəli, silinsin!',
            cancelButtonText: "Xeyr"
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => response.json())
                    .then(data => console.log(data));
                location.reload(true);
                Swal.fire(
                    'Silindi!',
                    'Seçdiyiniz iş silindi.',
                    'success'
                )
            }


        })
    })


    $(document).on("click", ".load-comments", function (e) {
        e.preventDefault();

        var nextPage = $(this).attr("data-nextpage");
        console.log(nextPage)
        var url = $(this).attr("href") + "?page=" + nextPage;

        console.log(url);
        fetch(url)
            .then(response => response.text())
            .then(data => {
                $(".Comments").append(data)
            });
        var totalPage = +$(this).data("totalpage");
        nextPage = +nextPage + 1;

        if (nextPage > totalPage) {
            $(this).remove();
        }
        $(this).attr("data-nextpage", nextPage)
    })

    // Slick Slider
    $(".responsive").slick({
        dots: true,
        infinite: false,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 2,
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: true,
                    dots: true,
                },
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2,
                },
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                },
            },
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ],
    });
    $(".responsiveSlider").slick({
        dots: true,
        infinite: true,
        speed: 300,
        slidesToShow: 1,
        slidesToScroll: 1,
        responsive: [
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ],
    });



    $(document).on("click", ".PortfolioXIcon", function () {
        //$(this).parents(".img-box").attr("style", "background-color:unset");
        $(this).parents(".PositionRelativeForIcon").css("display", "none ");

        $(this).parents(".PositionRelativeForIcon").empty();
    })
})
$(document).ready(function () {
    $(".ImageXIcon ").css("display", "flex ");

    $(".ImageXIcon").click(function (e) {
        e.preventDefault();
        $(".HiddenInputImage").css("display", "none ");
        $(".HiddenInputImage").attr("value", "");
        $(".ImageAndUpload img").css("display", "none ");
        $(".ImageXIcon ").css("display", "none ");
        $(this).parents(".ImageAndUpload").empty();
    });


    $(document).on("click", ".enum", function () {
        console.log("klik edildi");
        $(this).toggleClass("actived");
        var count = 0;
        for (var i = 0; i < document.getElementsByClassName("typeEnum").length; i++) {
            if (document.getElementsByClassName("typeEnum")[i].getAttribute("class") =="typeEnum enum actived") {
                document.getElementsByClassName("typeEnum")[i].setAttribute("name", `types[`+count+`]`);
                count++;
            }
        }
        var count1 = 0;
        for (var j = 0; j < document.getElementsByClassName("experienceEnum").length; j++) {
            if (document.getElementsByClassName("experienceEnum")[j].getAttribute("class") == "experienceEnum enum actived") {
                document.getElementsByClassName("experienceEnum")[j].setAttribute("name", `experiences[` + count1 + `]`);
                count1++;
            }
        }
        var count2 = 0;
        for (var t = 0; t < document.getElementsByClassName("levelEnum").length; t++) {
            if (document.getElementsByClassName("levelEnum")[t].getAttribute("class") == "levelEnum enum actived") {
                document.getElementsByClassName("levelEnum")[t].setAttribute("name", `careerLevels[` + count2 + `]`);
                count2++;
            }
        }
        var count3 = 0;
        for (var z = 0; z < document.getElementsByClassName("qualificationEnum").length; z++) {
            if (document.getElementsByClassName("qualificationEnum")[z].getAttribute("class") == "qualificationEnum enum actived") {
                document.getElementsByClassName("qualificationEnum")[z].setAttribute("name", `qualifications[` + count3 + `]`);
                count3++;
            }
        }
    });

    document.getElementsByClassName("AddEducationItem")[0].onclick = function () {
        var a = document.createElement("div");
        var education =
            ` 
                                <div class="CandidateResumeItem">
                                    <input type="hidden" class="EducationClickItem" name="CandidateEducationItems[2].Id" value="@item.Id" />
                                    <input type="hidden" class="EducationClickItem" name="CandidateEducationItems[2].CandidateId" value="@item.CandidateId" />
                                    <div class="accordion mt-3 border-radius">Yeni təhsil</div>
                                    <div class="panel mt-2">
                                        <p>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Başlıq</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 EducationClickItem" placeholder="Başlıq..." name="CandidateEducationItems[2].Title" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Təhsil aldığı məkan</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 EducationClickItem" placeholder="Təhsil müəssisəsi..." name="CandidateEducationItems[2].EducationPlace" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">İl aralığı</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 EducationClickItem" placeholder="İllər..." name="CandidateEducationItems[2].Years" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Məlumat</span></div>
                                                <div class="col-lg-9"><input style="min-height: 150px;max-height: 150px;" class="border-0 border-radius backColorBlue py-2 px-3 w-100 EducationClickItem" placeholder="Məlumat..." name="CandidateEducationItems[2].Content" ></div>
                                            </div>
                                            <div class="row">
                                                <div class="d-flex flex-row justify-content-end RemoveItem"><div class="btn backColorBlue py-2 px-2"><span class="blue">Remove</span></div></div>
                                            </div>
                                        </p>
                                    </div>
                                </div>
                                <br>
            `;
        a.innerHTML = education;
        document.getElementsByClassName("EducationItems")[0].appendChild(a);
        for (var i = 0; i < document.getElementsByClassName("EducationClickItem").length/6; i++) {
            document.getElementsByClassName("EducationClickItem")[i*6].setAttribute("name", `CandidateEducationItems[` + i + `].Id`);
            document.getElementsByClassName("EducationClickItem")[i*6+1].setAttribute("name", `CandidateEducationItems[` + i + `].CandidateId`);
            document.getElementsByClassName("EducationClickItem")[i*6+2].setAttribute("name", `CandidateEducationItems[` + i +`].Title`);
            document.getElementsByClassName("EducationClickItem")[i*6+3].setAttribute("name", `CandidateEducationItems[`+i+`].EducationPlace`);
            document.getElementsByClassName("EducationClickItem")[i*6+4].setAttribute("name", `CandidateEducationItems[` + i +`].Years`);
            document.getElementsByClassName("EducationClickItem")[i*6+5].setAttribute("name", `CandidateEducationItems[`+i+`].Content`);
        }

    }
    document.getElementsByClassName("AddWorkItem")[0].onclick = function () {

        var a = document.createElement("div");
        var work = `
                                 
                                <div class="CandidateResumeItem">
<input type="hidden" name="CandidateWorkItemsId[]" value="@item.Id"/>
                                    <input type="hidden" class="WorkClickItem" name="CandidateWorkItems[].Id" value="@item.Id" />
                                    <input type="hidden" class="WorkClickItem" name="CandidateWorkItems[].CandidateId" value="@item.CandidateId" />
                                    <div class="accordion mt-3 border-radius">Yeni iş/təcrübə</div>
                                    <div class="panel mt-2">
                                        <p>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Başlıq</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 WorkClickItem" placeholder="Başlıq..." name="CandidateWorkItems[].Title" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Başlama tarixi</span></div>
                                                <div class="col-lg-9"><input type="date" class="border-0 border-radius backColorBlue py-2 px-3 w-100 WorkClickItem" placeholder="Start Date" name="CandidateWorkItems[].StartDate" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Bitmə tarixi</span></div>
                                                <div class="col-lg-9"><input type="date" class="border-0 border-radius backColorBlue py-2 px-3 w-100 WorkClickItem" placeholder="End Date" name="CandidateWorkItems[].EndDate" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">İş yeri</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 WorkClickItem" placeholder="İş yeri..." name="CandidateWorkItems[].WorkPlace" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Məlumat</span></div>
                                                <div class="col-lg-9"><input style="min-height: 150px;max-height: 150px;" class="border-0 border-radius backColorBlue py-2 px-3 w-100 WorkClickItem" placeholder="Məlumat..." name="CandidateWorkItems[].Content" ></div>
                                            </div>
                                            <div class="row">
                                                <div class="d-flex flex-row justify-content-end RemoveItem"><div class="btn backColorBlue py-2 px-2"><span class="blue">Remove</span></div></div>
                                            </div>
                                        </p>
                                    </div>
                                </div>
                                <br>

`;
        a.innerHTML = work;
        document.getElementsByClassName("WorkItems")[0].appendChild(a);
        for (var i = 0; i < document.getElementsByClassName("WorkClickItem").length / 7; i++) {
            document.getElementsByClassName("WorkClickItem")[i * 7].setAttribute("name", `CandidateWorkItems[` + i + `].Id`);
            document.getElementsByClassName("WorkClickItem")[i * 7 + 1].setAttribute("name", `CandidateWorkItems[` + i + `].CandidateId`);
            document.getElementsByClassName("WorkClickItem")[i * 7 + 2].setAttribute("name", `CandidateWorkItems[` + i + `].Title`);
            document.getElementsByClassName("WorkClickItem")[i * 7 + 3].setAttribute("name", `CandidateWorkItems[` + i + `].StartDate`);
            document.getElementsByClassName("WorkClickItem")[i * 7 + 4].setAttribute("name", `CandidateWorkItems[` + i + `].EndDate`);
            document.getElementsByClassName("WorkClickItem")[i * 7 + 5].setAttribute("name", `CandidateWorkItems[` + i + `].WorkPlace`);
            document.getElementsByClassName("WorkClickItem")[i * 7 + 6].setAttribute("name", `CandidateWorkItems[` + i + `].Content`);
        }

    }
    document.getElementsByClassName("AddAwardItem")[0].onclick = function () {
        var a = document.createElement("div");
        var award = `
                                    

                                <div class="CandidateResumeItem">
 <input type="hidden" name="CandidateAwardItemsId[]" value="@item.Id"/>
                                    <input type="hidden" class="AwardClickItem" name="CandidateAwardItems[].Id" value="@item.Id" />
                                    <input type="hidden" class="AwardClickItem" name="CandidateAwardItems[].CandidateId" value="@item.CandidateId" />
                                    <div class="accordion mt-3 border-radius">Yeni Nailiyyət</div>
                                    <div class="panel mt-2">
                                        <p>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Başlıq</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 AwardClickItem" placeholder="Başlıq..." name="CandidateAwardItems[].Title" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">İl aralığı</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 AwardClickItem" placeholder="Məs: 2018-2022" name="CandidateAwardItems[].Years" ></div>
                                            </div>
                                            <br>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Məlumat</span></div>
                                                <div class="col-lg-9"><input style="min-height: 150px;max-height: 150px;" class="border-0 border-radius backColorBlue py-2 px-3 w-100 AwardClickItem" placeholder="Məlumat..." name="CandidateAwardItems[].Content" ></div>
                                            </div>
                                            <div class="row">
                                                <div class="d-flex flex-row justify-content-end RemoveItem"><div class="btn backColorBlue py-2 px-2"><span class="blue">Remove</span></div></div>
                                            </div>
                                        </p>
                                    </div>
                                </div>
                                <br>
`;
        a.innerHTML = award;
        document.getElementsByClassName("AwardItems")[0].appendChild(a);

        for (var i = 0; i < document.getElementsByClassName("AwardClickItem").length / 5; i++) {
            document.getElementsByClassName("AwardClickItem")[i * 5].setAttribute("name", `CandidateAwardItems[` + i + `].Id`);
            document.getElementsByClassName("AwardClickItem")[i * 5 + 1].setAttribute("name", `CandidateAwardItems[` + i + `].CandidateId`);
            document.getElementsByClassName("AwardClickItem")[i * 5 + 2].setAttribute("name", `CandidateAwardItems[` + i + `].Title`);
            document.getElementsByClassName("AwardClickItem")[i * 5 + 3].setAttribute("name", `CandidateAwardItems[` + i + `].Years`);
            document.getElementsByClassName("AwardClickItem")[i * 5 + 4].setAttribute("name", `CandidateAwardItems[` + i + `].Content`);
        }

    }
    document.getElementsByClassName("AddSkillItem")[0].onclick = function () {
        var a = document.createElement("div");
        var skill =
            ` 
                                                <div class="CandidateResumeItem">
                                    <input type="hidden" name="CandidateSkillsId[]" value="@item.Id" />
                                    <input type="hidden" class="SkillClickItem" name="CandidateSkills[].Id" value="@item.Id" />
                                    <input type="hidden" class="SkillClickItem" name="CandidateSkills[].CandidateId" value="@item.CandidateId" />
                                    <div class="accordion mt-3 border-radius">Yeni bilik</div>
                                    <div class="panel mt-2">
                                        <p>
                                            <div class="row ">
                                                <div class="col-lg-3 d-flex flex-row align-items-center"><span class="grey ">Title</span></div>
                                                <div class="col-lg-9"><input type="text" class="border-0 border-radius backColorBlue py-2 px-3 w-100 SkillClickItem" placeholder="Title" name="CandidateSkills[].Name" ></div>
                                            </div>
                                            <div class="row">
                                                <div class="d-flex flex-row justify-content-end RemoveItem"><div class="btn backColorBlue py-2 px-2"><span class="blue">Remove</span></div></div>
                                            </div>
                                        </p>
                                    </div>
                                </div>
                                <br>
            `;
        a.innerHTML = skill;
        document.getElementsByClassName("SkillItems")[0].appendChild(a);
        for (var i = 0; i < document.getElementsByClassName("SkillClickItem").length / 3; i++) {
            document.getElementsByClassName("SkillClickItem")[i * 3].setAttribute("name", `CandidateSkills[` + i + `].Id`);
            document.getElementsByClassName("SkillClickItem")[i * 3 + 1].setAttribute("name", `CandidateSkills[` + i + `].CandidateId`);
            document.getElementsByClassName("SkillClickItem")[i * 3 + 2].setAttribute("name", `CandidateSkills[` + i + `].Name`);
        }

    }


  $("#dropdownMenu2").click(function () {
    $(".dropdown-menu").slideToggle();
  });


});



for (
  let index = 0;
  index < document.getElementsByClassName("work-card-contentP").length;
  index++
) {
  if (
    document.getElementsByClassName("work-card-contentP")[index].scrollHeight >
    100
  ) {
    document.getElementsByClassName("work-card-contentP")[
      index
    ].style.maxHeight = "100px";
    document.getElementsByClassName("work-card-contentP")[
      index
    ].style.paddingRight = "15px";
    document.getElementsByClassName("work-card-contentP")[
      index
    ].style.overflow = "overlay";
  } else {
  }
}

var date = new Date();
document.getElementsByClassName("NowYear")[0].innerHTML = date.getFullYear();

for (
  let index = 0;
  index < document.getElementsByClassName("sponsorItem").length;
  index++
) {
  document.getElementsByClassName("sponsorItem")[index].onmouseover =
    function () {
      for (
        let i = 0;
        i < document.getElementsByClassName("sponsorItem").length;
        i++
      ) {
        if (i != index) {
          document.getElementsByClassName("sponsorItem")[i].style.width =
            "60px";
          document.getElementsByClassName("sponsorItem")[i].style.height =
            "20px";
          document.getElementsByClassName("sponsorItem")[
            i
          ].style.transitionDuration = "0.3s";
        }
      }
    };
  document.getElementsByClassName("sponsorItem")[index].onmouseout =
    function () {
      for (
        let i = 0;
        i < document.getElementsByClassName("sponsorItem").length;
        i++
      ) {
        if (i != index) {
          document.getElementsByClassName("sponsorItem")[i].style.width =
            "100px";
          document.getElementsByClassName("sponsorItem")[i].style.height =
            "30px";
          document.getElementsByClassName("sponsorItem")[
            i
          ].style.transitionDuration = "0.3s";
        }
      }
    };
}
$(document).ready(function () {

    //var acc = document.getElementsByClassName("accordion");
    //var i;

    //for (i = 0; i < acc.length; i++) {
    //    acc[i].addEventListener("click", function () {
    //        console.log("salam");
    //        this.classList.toggle("active");

    //        var panel = this.nextElementSibling;
    //        if (panel.style.maxHeight) {
    //            panel.style.maxHeight = null;
    //        } else {
    //            panel.style.maxHeight = panel.scrollHeight + "px";
    //        }
    //    });
    //}

    $(document).ready(function () {

        for (var i = 0; i < document.getElementsByClassName("RemoveItem").length; i++) {
            $(document).on("click", ".RemoveItem", function (e) {
                e.preventDefault();
                //this.parentElement.parentElement.parentElement.style.display = "none";
                $(this).parents(".CandidateResumeItem").empty();
                for (var i = 0; i < document.getElementsByClassName("EducationClickItem").length / 6; i++) {
                    document.getElementsByClassName("EducationClickItem")[i * 6].setAttribute("name", `CandidateEducationItems[` + i + `].Id`);
                    document.getElementsByClassName("EducationClickItem")[i * 6 + 1].setAttribute("name", `CandidateEducationItems[` + i + `].CandidateId`);
                    document.getElementsByClassName("EducationClickItem")[i * 6 + 2].setAttribute("name", `CandidateEducationItems[` + i + `].Title`);
                    document.getElementsByClassName("EducationClickItem")[i * 6 + 3].setAttribute("name", `CandidateEducationItems[` + i + `].EducationPlace`);
                    document.getElementsByClassName("EducationClickItem")[i * 6 + 4].setAttribute("name", `CandidateEducationItems[` + i + `].Years`);
                    document.getElementsByClassName("EducationClickItem")[i * 6 + 5].setAttribute("name", `CandidateEducationItems[` + i + `].Content`);
                }
                for (var i = 0; i < document.getElementsByClassName("WorkClickItem").length / 7; i++) {
                    document.getElementsByClassName("WorkClickItem")[i * 7].setAttribute("name", `CandidateWorkItems[` + i + `].Id`);
                    document.getElementsByClassName("WorkClickItem")[i * 7 + 1].setAttribute("name", `CandidateWorkItems[` + i + `].CandidateId`);
                    document.getElementsByClassName("WorkClickItem")[i * 7 + 2].setAttribute("name", `CandidateWorkItems[` + i + `].Title`);
                    document.getElementsByClassName("WorkClickItem")[i * 7 + 3].setAttribute("name", `CandidateWorkItems[` + i + `].StartDate`);
                    document.getElementsByClassName("WorkClickItem")[i * 7 + 4].setAttribute("name", `CandidateWorkItems[` + i + `].EndDate`);
                    document.getElementsByClassName("WorkClickItem")[i * 7 + 5].setAttribute("name", `CandidateWorkItems[` + i + `].WorkPlace`);
                    document.getElementsByClassName("WorkClickItem")[i * 7 + 6].setAttribute("name", `CandidateWorkItems[` + i + `].Content`);
                }
                for (var i = 0; i < document.getElementsByClassName("AwardClickItem").length / 5; i++) {
                    document.getElementsByClassName("AwardClickItem")[i * 5].setAttribute("name", `CandidateAwardItems[` + i + `].Id`);
                    document.getElementsByClassName("AwardClickItem")[i * 5 + 1].setAttribute("name", `CandidateAwardItems[` + i + `].CandidateId`);
                    document.getElementsByClassName("AwardClickItem")[i * 5 + 2].setAttribute("name", `CandidateAwardItems[` + i + `].Title`);
                    document.getElementsByClassName("AwardClickItem")[i * 5 + 3].setAttribute("name", `CandidateAwardItems[` + i + `].Years`);
                    document.getElementsByClassName("AwardClickItem")[i * 5 + 4].setAttribute("name", `CandidateAwardItems[` + i + `].Content`);
                }
                for (var i = 0; i < document.getElementsByClassName("SkillClickItem").length / 3; i++) {
                    document.getElementsByClassName("SkillClickItem")[i * 3].setAttribute("name", `CandidateSkills[` + i + `].Id`);
                    document.getElementsByClassName("SkillClickItem")[i * 3 + 1].setAttribute("name", `CandidateSkills[` + i + `].CandidateId`);
                    document.getElementsByClassName("SkillClickItem")[i * 3 + 2].setAttribute("name", `CandidateSkills[` + i + `].Name`);
                }
            })
        }

    })
    $(document).ready(function () {
        $(document).on("click", ".accordion", function (e) {
            e.preventDefault();
            this.classList.toggle("active");

            var panel = this.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
        });
    });

})
$(document).ready(function () {
  $(".Advanced").click(function () {
    $(".DropdownOnclickAdvanced").fadeToggle("slow");
  });
});
$(document).ready(function () {
  $(".SortBy").click(function () {
    $(".SortingBy").fadeToggle("slow");
  });
});
$(document).ready(function () {
  $(".AllClick").click(function () {
    $(".All").fadeToggle("slow");
  });
});

// for (
//   let index = 0;
//   index < document.getElementsByClassName("UserProfileItem").length;
//   index++
// ) {
//   console.log(index);

//   document.getElementsByClassName("UserProfileItem")[index].onclick =
//     function () {
//       document.getElementsByClassName("UserProfileItem")[
//         index
//       ].style.backgroundColor = "#E8F0FA ";
//       document.getElementsByClassName("UserProfileItem")[index].style.color =
//         "blue ";
//       for (
//         let i = 0;
//         i < document.getElementsByClassName("UserProfileItem").length;
//         i++
//       ) {
//         if (i != index) {
//           document.getElementsByClassName("UserProfileItem")[
//             i
//           ].style.backgroundColor = "white ";
//           document.getElementsByClassName("UserProfileItem")[i].style.color =
//             "grey ";
//         }
//       }
//     };
// }
// anychart.onDocumentReady(function () {
//   // set the data
//   var data = {
//     header: ["Year", "Interest Expense on the Debt, USD bln."],
//     rows: [
//       [2007, 420],
//       [2008, 451],
//       [2009, 430],
//       [2010, 413],
//       [2011, 454],
//       [2012, 359],
//       [2013, 415],
//       [2014, 430],
//       [2015, 402],
//       [2016, 432],
//     ],
//   };

//   // create the chart
//   var chart = anychart.area();

//   // add the data
//   chart.data(data);

//   chart.container("container");
//   chart.draw();
// });

//$(".duyme1").click(function () {
//  $(".leftSection").attr("class", "col-lg-9 py-5 leftSection");
//  $(".sidebar").fadeToggle();
//});

//for (var i = 0; i < document.getElementsByClassName("RemoveItem").length; i++) {
//    document.getElementsByClassName("RemoveItem")[i].addEventListener("click", function () {
//        this.parentElement.parentElement.parentElement.style.display = "none";
//    });
//}


//for (var i = 0; i < document.getElementsByClassName("RemoveImageItem").length; i++) {

//    document.getElementsByClassName("RemoveImageItem")[i].addEventListener("click", function () {
//        this.parentElement.style.display = "none ";
//    });
//}
for (var i = 0; i < document.getElementsByClassName("RemoveCVItem").length; i++) {

    document.getElementsByClassName("RemoveCVItem")[i].parentElement.style.display = "flex ";
    document.getElementsByClassName("RemoveCVItem")[i].parentElement.parentElement.style.display = "flex ";
}




//for (var i = 0; i < document.getElementsByClassName("accordion").length; i++) {
//    document.getElementsByClassName("accordion")[i].onclick = function () {
//        this.nextElementSibling.slideToggle();
//    }
//}


$(document).ready(function () {

    $(document).on("click", ".RemoveCVItem", function (e) {
        e.preventDefault();
        //$(this).parents(".img-box").attr("style", "background-color:unset");
        $(this).parents(".RemoveCVItems").empty();
    })
})



$(document).ready(function () {


    ClassicEditor
        .create(document.querySelector('#editor'))
        .catch(error => {
            console.error(error);
        });
    InlineEditor
        .create(document.querySelector('#editor'))
        .catch(error => {
            console.error(error);
        });
    BalloonEditor
        .create(document.querySelector('#editor'))
        .catch(error => {
            console.error(error);
        });
    DecoupledEditor
        .create(document.querySelector('#editor'))
        .then(editor => {
            const toolbarContainer = document.querySelector('#toolbar-container');

            toolbarContainer.appendChild(editor.ui.view.toolbar.element);
        })
        .catch(error => {
            console.error(error);
        });
})


