

document.getElementById("reserveBtn").addEventListener("click", function () {
   
    let roomsData = [];
    let IsVaildationRight = true;

    document.querySelectorAll(".roomCount").forEach(roomInput => {
        let roomId = roomInput.dataset.roomid;
        let numRooms = parseInt(roomInput.value) || 0;

        if (numRooms > 0) {
            let guestConfig = document.getElementById(`guestConfig-${roomId}`);
            let roomDetails = [];

            let guestRows = guestConfig.querySelectorAll(".row");
            for (let row of guestRows) {
                let adults = parseInt(row.querySelector(".adults").value) || 0;
                let children = parseInt(row.querySelector(".children").value) || 0;

                // Validation: Each room must have at least one guest
                if (adults + children === 0) {
                    alert(`each Room must have at least one person (adult or child).`);
                    IsVaildationRight = false;
                    return; // Stop execution completely
                }

                roomDetails.push(`${adults},${children}`);
            }
           
            roomsData.push(`${roomId}-${roomDetails.join("/")}`);
        }
    });

    if (IsVaildationRight) {
        if (roomsData.length > 0) {
        let finalData = `${roomsData.join("-")}`;

        console.log("Sending data:", finalData); 

        ReservationForm(finalData);
    } else {
        alert("Please select at least one room before reserving.");
    }
    }
});

function ReservationForm(data) {

    // Get the modal element
    const myModal = new bootstrap.Modal(document.getElementById('myModal'));

    document.getElementById("ReservationRoomsData").value = data;
    
    myModal.show();

}
