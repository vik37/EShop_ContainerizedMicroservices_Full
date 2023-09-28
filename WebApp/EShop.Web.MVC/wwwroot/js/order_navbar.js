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
        { id: "account-settings-link", orderNavbarListIsOpen: false },
        { id: "order-link", orderNavbarListIsOpen: false },
        { id: "coupon-link", orderNavbarListIsOpen: false },
        { id: "order-history-link", orderNavbarListIsOpen: false },
    ];


    function setInitialOrderListNavbarToggleHeadingModification(id, orderNavbarListIsOpen) {
        if (!orderNavbarListIsOpen) {
            $($(`#${id}-title`)).removeClass("orders-link-title-opened").addClass("orders-link-title-closed");
            $($(`#${id}-block`)).removeClass("order-link-list-block-opened").addClass("order-link-list-block-closed");
        }
        else {
            $($(`#${id}-title`)).removeClass("orders-link-title-closed").addClass("orders-link-title-opened");
            $($(`#${id}-block`)).removeClass("order-link-list-block-closed").addClass("order-link-list-block-opened");
        }
    }

    let orderSummaryHeading = $("h2").attr("id");

    $(".orders-link-title > span").each(function (index) {

        switch (orderSummaryHeading) {
            case "account-settings-personal-information-heading":
                orderLinkBloksIdsObj[0].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                break;
            case "order-address-heading":
                orderLinkBloksIdsObj[0].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                break;
            case "order-summary-heading":
                orderLinkBloksIdsObj[1].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                console.log(index);
                break;
            case "order-detail-heading":
                orderLinkBloksIdsObj[1].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                
                break;
            case "all-ordered-products-heading":
                orderLinkBloksIdsObj[1].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                break;
            case "order-overall-analysis-heading":
                orderLinkBloksIdsObj[1].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                break;
            case "order-coupon-heading":
                orderLinkBloksIdsObj[2].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                break;
            case "order-history-heading":
                orderLinkBloksIdsObj[3].orderNavbarListIsOpen = true;
                setInitialOrderListNavbarToggleHeadingModification(orderLinkBloksIdsObj[index].id, orderLinkBloksIdsObj[index].orderNavbarListIsOpen);
                break;
        }

        $(this).on("click", function () {

            orderLinkBloksIdsObj[index].orderNavbarListIsOpen = !orderLinkBloksIdsObj[index].orderNavbarListIsOpen;

            $($(`#${orderLinkBloksIdsObj[index].id}-block`)).slideToggle("slow");             

            orderListNavbarToggleHeadingModification(index);
        })
    });     

    function orderListNavbarToggleHeadingModification(index) {
        if (!orderLinkBloksIdsObj[index].orderNavbarListIsOpen) {
            $($(`#${orderLinkBloksIdsObj[index].id}-title`)).removeClass("orders-link-title-opened").addClass("orders-link-title-closed");
        }
        else {
            $($(`#${orderLinkBloksIdsObj[index].id}-title`)).removeClass("orders-link-title-closed").addClass("orders-link-title-opened");
        }                
    }
    
})