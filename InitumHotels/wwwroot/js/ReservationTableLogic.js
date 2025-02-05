document.getElementById("statusFilter").addEventListener("change", function () {
    let selectedStatus = this.value;
    let rows = document.querySelectorAll("tbody tr");

    rows.forEach(row => {
        let rowStatus = row.getAttribute("data-status");
        row.style.display = (selectedStatus === "All" || rowStatus === selectedStatus) ? "" : "none";
    });
});

function loadReservationDetails(id, name, phone, email, nationalId, discount, branch, checkIn, checkOut, status, total, roomsjson) {
    document.getElementById("reservationId").value = id;
    document.getElementById("customerName").textContent = name;
    document.getElementById("phoneNumber").textContent = phone;
    document.getElementById("email").textContent = email;
    document.getElementById("nationalId").textContent = nationalId;
    document.getElementById("discountOn").textContent = discount ? "Yes" : "No";
    document.getElementById("hotelBranchName").textContent = branch;
    document.getElementById("checkInDate").textContent = checkIn;
    document.getElementById("checkOutDate").textContent = checkOut;
    document.getElementById("status").value = status;
    document.getElementById("Total").textContent = total + '$';

    populateRooms(roomsjson);
}

function populateRooms(roomsJson) {
    let roomsContainer = document.getElementById("roomsContainer");
    roomsContainer.innerHTML = ""; // Clear existing content

    let rooms = JSON.parse(roomsJson); // Parse the JSON data

    rooms.forEach(room => {
        let roomCard = `
                <div class="card shadow-sm mb-2">
                    <div class="card-header bg-secondary text-white">
                        <strong>${room.Name}</strong>
                    </div>
                    <div class="card-body">
                        <strong>Room Numbers:</strong>
                    <ul class="list-group mt-2">
                        ${room.RoomNumbers.map(num => `<li class="list-group-item">${num}</li>`).join("")}
                    </ul>
                    </div>
                </div>
            `;
        roomsContainer.innerHTML += roomCard;
    });
}