$(document).ready(function () {
  let allowSubmit = false;
  $('.shipping-method-next-step-button').on('click', function (e) {
    const selectedShipping = $('input[name="shippingoption"]:checked').val();
    if (selectedShipping && selectedShipping.includes("Shipping.BoxNow")) {
      if (!allowSubmit) {
        e.preventDefault();
        $('.boxnow-map-widget-button').click();
      }
    }
  });
  $('#confirmShippingBtn').on('click', function () {
    allowSubmit = true;
    $('.shipping-method-next-step-button').click();
  });
});