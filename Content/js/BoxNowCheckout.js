let allowSubmit = false;
$(document).ready(function () {
  $('.shipping-method-next-step-button').on('click', function (e) {
    const selectedShipping = $('input[name="shippingoption"]:checked').val();
    if (selectedShipping && selectedShipping.includes("Shipping.BoxNow")) {
      if (!allowSubmit) {
        e.preventDefault();
        document.querySelector('.boxnow-map-widget-button').click();
      }
    }
  });
});
var _bn_map_widget_config = {
  type: "popup",
  autoselect: false,
  autoclose: true,
  partnerId: "@Model.PartnerID",
  parentElement: "#boxnowmap",
  afterSelect: function (selected) {
    allowSubmit = true;
    var data = {
      LockerID: selected.boxnowLockerId,
      AddressLine1: selected.boxnowLockerAddressLine1,
      PostalCode: selected.boxnowLockerPostalCode
    };
    fetch("/box-now/selected-item", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(data)
    })
      .then(response => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then(result => {
        console.log("Success:", result);
      })
      .catch(error => {
        console.error("Error sending data:", error);
      });
  }
};
(function (d) { var e = d.createElement("script"); e.src = "https://widget-cdn.boxnow.gr/map-widget/client/v5.js"; e.async = true; e.defer = true; d.getElementsByTagName("head")[0].appendChild(e); })(document);