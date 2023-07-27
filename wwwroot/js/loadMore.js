let skip = 6;
let coursesCount = $("#loadMore").next().val()
$(document).on("click", "#loadMore", function () {
    $.ajax({
        url: "/Courses/LoadMore/",
        type: "GET",
        data: {
            "skip": skip
        },
        success: function (res) {

            skip += 6
            if (coursesCount <= skip) {
                $("#loadMore").remove()
            }
            $("#myCourse").append(res)
        }
    });
});