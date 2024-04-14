namespace Gbc_Travel_Group63.wwwroot.js {
    <html>
    <style>
        .loader {
            border: 8px solid #f3f3f3;
            border-radius: 50%;
            border-top: 8px solid #3498db;
            width: 60px;
            height: 60px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
    }

     @-webkit-keyframes spin {
                0 % { - webkit - transform: rotate(0deg); }
            100% {-webkit - transform: rotate(360deg); }
}

        @keyframes spin {
            0 % { transform: rotate(0deg); },
            100% {transform: rotate(360deg); }
        }
        </style>

    </html>
    $(document).ready(function () {
        // Flight search form submission
        $('#flight-search-form').submit(function (event) {
            event.preventDefault();
            var formData = $(this).serialize();
            // Show loader/spinner
            $('#search-results').html('<div class="loader"></div>');
            $.ajax({
                type: 'POST',
                url: '/flights/search',
                data: formData,
                success: function (data) {
                    // Update search results section with received data
                    $('#search-results').html(data);
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.log(error);
                },
                complete: function () {
                    // Hide loader/spinner
                    $('#search-results .loader').remove();
                }
            });
        });

        // Hotel search form submission
        $('#hotel-search-form').submit(function (event) {
            event.preventDefault();
            var formData = $(this).serialize();
            // Show loader/spinner
            $('#search-results').html('<div class="loader"></div>');
            $.ajax({
                type: 'POST',
                url: '/hotels/search',
                data: formData,
                success: function (data) {
                    // Update search results section with received data
                    $('#search-results').html(data);
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.log(error);
                },
                complete: function () {
                    // Hide loader/spinner
                    $('#search-results .loader').remove();
                }
            });
        });
        $(document).ready(function () {
            $('#book-flight-btn').click(function () {
                var flightId = $(this).data('flight-id');
                $.ajax({
                    type: 'POST',
                    url: '/flights/book', // Endpoint to handle flight booking
                    data: { flightId: flightId },
                    success: function (response) {
                        // Update relevant section of the page with booking confirmation
                        $('#booking-details').html(response);
                    },
                    error: function (xhr, status, error) {
                        // Handle error
                        console.error(error);
                    }
                });
            });
        });


        // Car rental search form submission
        $('#car-search-form').submit(function (event) {
            event.preventDefault();
            var formData = $(this).serialize();
            // Show loader/spinner
            $('#search-results').html('<div class="loader"></div>');
            $.ajax({
                type: 'POST',
                url: '/carrentals/search',
                data: formData,
                success: function (data) {
                    // Update search results section with received data
                    $('#search-results').html(data);
                },
                error: function (xhr, status, error) {
                    // Handle error
                    console.log(error);
                },
                complete: function () {
                    // Hide loader/spinner
                    $('#search-results .loader').remove();
                }
            });
        });
    });
}
