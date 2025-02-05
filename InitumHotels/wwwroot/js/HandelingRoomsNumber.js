
    document.addEventListener("DOMContentLoaded", function () {
        let totalPrice = 0;
    let totalRooms = 0;

        function updateSummary() {

            const start = new Date(document.getElementById('CheckInDate').value);
            const end = new Date(document.getElementById('CheckOutDate').value);

            // Set time to midnight to ignore time comparison
            start.setHours(0, 0, 0, 0);
            end.setHours(0, 0, 0, 0);

            // Calculate the time difference in milliseconds
            const timeDiff = end - start;

            // Convert time difference from milliseconds to days
            const dayDiff = (timeDiff / (1000 * 3600 * 24)) + 1;

            document.getElementById("totalPrice").textContent = "$" + (totalPrice * dayDiff);
            document.getElementById("totalRooms").textContent = totalRooms;
        }

        document.querySelectorAll(".roomCount").forEach(input => {
        input.addEventListener("input", function () {
            let roomId = this.dataset.roomid;
            let roomPrice = parseInt(this.dataset.price);
            let maxAdult = parseInt(this.dataset.maxadult);
            let maxChildren = parseInt(this.dataset.maxchildren);
            let numRooms = parseInt(this.value) || 0;

            totalRooms = 0;
            totalPrice = 0;

            document.querySelectorAll(".roomCount").forEach(roomInput => {
                let count = parseInt(roomInput.value) || 0;
                totalRooms += count;
                totalPrice += count * parseInt(roomInput.dataset.price);
            });

            updateSummary();

            let guestConfig = document.getElementById(`guestConfig-${roomId}`);
            guestConfig.innerHTML = "";

            for (let i = 1; i <= numRooms; i++) {
                guestConfig.innerHTML += `
                        <div class="row mb-2">
                            <div class="col-md-6">
                                <label>Adults in Room ${i}</label>
                                <input type="number" class="form-control restricted-number adults" min="0" max="${maxAdult}" value="0">
                            </div>
                            <div class="col-md-6">
                                <label>Children in Room ${i}</label>
                                <input type="number" class="form-control restricted-number children" min="0" max="${maxChildren}" value="0">
                            </div>
                        </div>`;
            }

            // Event delegation for dynamically added inputs
            guestConfig.addEventListener('change', function (e) {
                if (e.target && e.target.classList.contains('restricted-number')) {
                    const input = e.target;
                    const min = parseFloat(input.getAttribute('min'));
                    const max = parseFloat(input.getAttribute('max'));

                    // Check if the input value is below the minimum or above the maximum
                    if (parseFloat(input.value) < min) {
                        input.value = min;  // Set to the min value if below
                    } else if (parseFloat(input.value) > max) {
                        input.value = max;
                    }
                }
            });
        });
                    });
    });