

        document.addEventListener("DOMContentLoaded", function () {
            // Get today's date in YYYY-MM-DD format
            let today = new Date().toISOString().split('T')[0];

        // Set the minimum date for check-in
        let CheckInDate = document.getElementById("CheckInDate");
        let CheckOutDate = document.getElementById("CheckOutDate");
        CheckInDate.setAttribute("min", today);

        // Prevent selecting a check-out date earlier than the check-in date
        CheckInDate.addEventListener("change", function () {
            let CheckInDateDate = this.value;

        let nextDay = new Date(CheckInDateDate);
        nextDay.setDate(nextDay.getDate() + 1);

        let minCheckOutDate = nextDay.toISOString().split("T")[0]; // Convert back to YYYY-MM-DD format


        CheckOutDate.setAttribute("min", minCheckOutDate);

        // If check-out date is set and it's earlier than the new check-in date, reset check-out date
        if (CheckOutDate.value && CheckOutDate.value < CheckInDateDate) {
            CheckOutDate.value = "";
        CheckOutDate.setAttribute("placeholder", "Select check-out date");
            }
        });

        // Compare check-in and check-out dates if both are selected
        CheckOutDate.addEventListener("change", function () {
            let CheckInDateDate = CheckInDate.value;
        let CheckOutDateDate = CheckOutDate.value;

        if (CheckInDateDate && CheckOutDateDate && CheckOutDateDate < CheckInDateDate) {
            CheckOutDate.value = ""; // Reset the check-out date if invalid
        CheckOutDate.setAttribute("placeholder", "Select check-out date");
            }
        });
    });