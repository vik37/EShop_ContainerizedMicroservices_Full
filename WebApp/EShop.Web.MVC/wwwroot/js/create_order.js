$(document).ready(() => {
    const degreesNumber = 90;
    let isShoppingHidden = false;
    let isPaymentHidden = true;
    let isOrderDetailsHidden = true;
    $(".show-shopping > span").on("click", function () {
        $(".shopping").slideToggle("slow");
        isShoppingHidden = !isShoppingHidden;
        changeRotationTransformation(isShoppingHidden, "shopping", degreesNumber);
    });
    $(".show-payment > span").on("click", function () {
        $(".payment").slideToggle("slow");
        isPaymentHidden = !isPaymentHidden;
        changeRotationTransformation(isPaymentHidden, "payment", degreesNumber);
    });
    $(".show-order-details > span").on("click", function () {
        $(".order-summary-details").slideToggle("slow");
        isOrderDetailsHidden = !isOrderDetailsHidden;
        changeRotationTransformation(isOrderDetailsHidden, "order-details", degreesNumber);
        $(".show-order-details-text").css("display", "none");
    });

    function changeRotationTransformation(hidden, showTypeClass, degreesNumber) {
        let negativeDegreesNumber = degreesNumber - (degreesNumber * 2);
        if (hidden) {
            $(`.show-${showTypeClass} > span`).css("transform", `rotate(${negativeDegreesNumber}deg)`);
        }
        else {
            $(`.show-${showTypeClass} > span`).css("transform", `rotate(${degreesNumber}deg)`);
        }
    }
})