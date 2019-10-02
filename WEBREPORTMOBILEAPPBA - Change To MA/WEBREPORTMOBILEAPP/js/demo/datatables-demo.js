// Call the dataTables jQuery plugin
$(document).ready(function() {
    $('#dataTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel'
        ],
        "paging": false,
        "pageLength": 50,
        "lengthMenu": [[50, 100, 500, -1], ["50", "100", "500", "All"]],
        "ordering": true,
        "info": true
    }
        
    );
});
