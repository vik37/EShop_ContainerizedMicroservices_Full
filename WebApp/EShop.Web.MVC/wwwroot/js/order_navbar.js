$(document).ready(() => {

    let orderNavbarIsOpen = true;

    $(".customer-order-nav-button-toggle").on("click", function () {
        orderNavbarIsOpen = !orderNavbarIsOpen;
        orderNavbarToggle();
    });

    function orderNavbarToggle() {

        if (!orderNavbarIsOpen) {
            $(".customer-order-nav-button-toggle > i").replaceWith("<i class='fa-solid fa-layer-group'></i>");
            $(".customer-order-navbar-block").animate({
                width: "35px"
            }, { duration: 600 });
            $(".customer-order-navbar > h6").fadeOut(199, "linear");
            $(".order-navbar-links-block").fadeOut(200);
            $(".customer-order-nav-button-toggle-open").off("click", "**");
        }
        else {
            $(".customer-order-nav-button-toggle > i").replaceWith(`<i class="fa-regular fa-map">`);
            $(".customer-order-navbar-block").animate({
                width: "200px"
            }, { duration: 600 });
            $(".customer-order-navbar > h6").fadeIn("slow", "linear");
            $(".order-navbar-links-block").delay(300).fadeIn(1000);
            $(".customer-order-nav-button-toggle-close").off("click", "**");
        }
    }

    let orderLinkBloksIdsObj = [
        { id: "account-settings-link", orderNabarListIsOpen: true },
        { id: "order-link", orderNabarListIsOpen: false },
        { id: "coupon-link", orderNabarListIsOpen: false },
        { id: "order-history-link", orderNabarListIsOpen: false },
    ];

    $(".orders-link-title > span").each(function (index) {
        $(this).on("click", function () {

            orderLinkBloksIdsObj[index].orderNabarListIsOpen = !orderLinkBloksIdsObj[index].orderNabarListIsOpen;

            $($(`#${orderLinkBloksIdsObj[index].id}-block`)).slideToggle("slow");             

            orderListNavbarToggleModification(index);
        })
    });

    function orderListNavbarToggleModification(index) {
        if (!orderLinkBloksIdsObj[index].orderNabarListIsOpen) {
            $($(`#${orderLinkBloksIdsObj[index].id}-title`)).removeClass("orders-link-title-opened").addClass("orders-link-title-closed");
        }
        else {
            $($(`#${orderLinkBloksIdsObj[index].id}-title`)).removeClass("orders-link-title-closed").addClass("orders-link-title-opened");
        }                
    }
})