function paymentMethodChangeEvent(paymentType) {
    if (paymentType == "credit") {
        var paymentBox = document.getElementById("payment-box");
        paymentBox.style.display = "block";
    }
    else {
        var paymentBox = document.getElementById("payment-box");
        paymentBox.style.display = "none";
    }
}