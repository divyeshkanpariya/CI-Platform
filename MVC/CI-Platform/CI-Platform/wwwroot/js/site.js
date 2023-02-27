$(".btn-list").click(function showList(e) {
    document.getElementById("listButton").style.backgroundColor = "#DDDDDD";
    document.getElementById("gridButton").style.backgroundColor = "white";

    var $gridCont = $(".grid-container");
    e.preventDefault();
    if (!$gridCont.hasClass("list-view")) {
        $gridCont.addClass("list-view");
    }
    /* $gridCont.hasClass('list-view') ? $gridCont.removeClass('list-view') : $gridCont.addClass('list-view');*/


});
$(".btn-grid").click(function gridList(e) {
    document.getElementById("gridButton").style.backgroundColor = "#DDDDDD";
    document.getElementById("listButton").style.backgroundColor = "white";
    var $gridCont = $(".grid-container");
    e.preventDefault();
    $gridCont.removeClass("list-view");


});

/* Image Slider Volunteer Mission */

$('#myCarousel').carousel({
    interval: false
});
$('#carousel-thumbs').carousel({
    interval: false
});

// handles the carousel thumbnails
// https://stackoverflow.com/questions/25752187/bootstrap-carousel-with-thumbnails-multiple-carousel
$('[id^=carousel-selector-]').click(function () {
    var id_selector = $(this).attr('id');
    var id = parseInt(id_selector.substr(id_selector.lastIndexOf('-') + 1));
    $('#myCarousel').carousel(id);
});
// Only display 3 items in nav on mobile.
if ($(window).width() < 575) {
    $('#carousel-thumbs .row div:nth-child(4)').each(function () {
        var rowBoundary = $(this);
        $('<div class="row mx-0">').insertAfter(rowBoundary.parent()).append(rowBoundary.nextAll().addBack());
    });
    $('#carousel-thumbs .carousel-item .row:nth-child(even)').each(function () {
        var boundary = $(this);
        $('<div class="carousel-item">').insertAfter(boundary.parent()).append(boundary.nextAll().addBack());
    });
}
// Hide slide arrows if too few items.
if ($('#carousel-thumbs .carousel-item').length < 2) {
    $('#carousel-thumbs [class^=carousel-control-]').remove();
    $('.machine-carousel-container #carousel-thumbs').css('padding', '0 5px');
}
// when the carousel slides, auto update
$('#myCarousel').on('slide.bs.carousel', function (e) {
    var id = parseInt($(e.relatedTarget).attr('data-slide-number'));
    $('[id^=carousel-selector-]').removeClass('selected');
    $('[id=carousel-selector-' + id + ']').addClass('selected');
});
// when user swipes, go next or previous
$('#myCarousel').swipe({
    fallbackToMouseEvents: true,
    swipeLeft: function () {
        $('#myCarousel').carousel('next');
    },
    swipeRight: function () {
        $('#myCarousel').carousel('prev');
    },
    allowPageScroll: 'vertical',
    preventDefaultEvents: false,
    threshold: 75
});
/*
$(document).on('click', '[data-toggle="lightbox"]', function(event) {
  event.preventDefault();
  $(this).ekkoLightbox();
});
*/

$('#myCarousel .carousel-item img').on('click', function (e) {
    var src = $(e.target).attr('data-remote');
    if (src) $(this).ekkoLightbox();
});

function postComment() {
    console.log("sdes");
    const child = document.getElementById("commentList").childElementCount;

    var text = document.getElementById("commentInp");
    console.log(text.value);
    var commentList = document.getElementById("commentList");

    var mainDiv = document.createElement("div");
    mainDiv.classList.add("d-flex", "comment", "bg-white", "p-2", "align-items-center", "border", "mb-3");

    var img = document.createElement("img");
    img.src = "/images/volunteer1.png";
    mainDiv.appendChild(img);

    var commentTexts = document.createElement("div");
    commentTexts.classList.add("ms-3");

    var name = document.createElement("h5");
    name.innerHTML = "Kane Williamson";
    commentTexts.appendChild(name);

    var time = document.createElement("p");
    time.innerHTML = new Date();
    commentTexts.appendChild(time);

    var cmtext = document.createElement("p");
    cmtext.innerHTML = text.value;
    commentTexts.appendChild(cmtext);

    mainDiv.appendChild(commentTexts);
    if (child != 0) {
        var firstchild = document.getElementById("commentList").firstChild;
        commentList.insertBefore(mainDiv, firstchild);
    } else {
        commentList.appendChild(mainDiv);
    }

}
function noMissionFound() {
    document.getElementById("missionContainer").style.display = "none";
    document.getElementById("paginationContainer").style.display = "none";
    document.getElementById("viewContainer").style.display = "none";
    document.getElementById("addedFilters").style.display = "none";
    document.getElementById("noMissionFound").style.display = "block";

}

function nextVol() {
    console.log("sdesdwe");

    var tab1 = document.getElementById("recentVolTab1");
    var tab2 = document.getElementById("recentVolTab2");
    var tab3 = document.getElementById("recentVolTab3");
    var text = document.getElementById("currentVol");


    if (tab2.style.display == "flex") {
        tab1.style.display = "none";
        tab3.style.display = "flex";
        tab2.style.display = "none";
        text.innerHTML = "19 - 25 of 25 Recent Volunteers";
    } else if (tab3.style.display == "flex") {
        tab1.style.display = "flex";
        tab3.style.display = "none";
        tab2.style.display = "none";
        text.innerHTML = "1 - 9 of 25 Recent Volunteers";
    } else {
        tab1.style.display = "none";
        tab3.style.display = "none";
        tab2.style.display = "flex";
        text.innerHTML = "10 - 18 of 25 Recent Volunteers";
    }
}

function previousVol() {
    var tab1 = document.getElementById("recentVolTab1");
    var tab2 = document.getElementById("recentVolTab2");
    var tab3 = document.getElementById("recentVolTab3");
    var text = document.getElementById("currentVol");

    if (tab2.style.display == "flex") {
        tab1.style.display = "flex";
        tab3.style.display = "none";
        tab2.style.display = "none";
        text.innerHTML = "1 - 9 of 25 Recent Volunteers";

    } else if (tab3.style.display == "flex") {
        tab1.style.display = "none";
        tab3.style.display = "none";
        tab2.style.display = "flex";
        text.innerHTML = "10 - 18 of 25 Recent Volunteers";
    } else {
        tab1.style.display = "none";
        tab3.style.display = "flex";
        tab2.style.display = "none";
        text.innerHTML = "19 - 27 of 25 Recent Volunteers";
    }
}
