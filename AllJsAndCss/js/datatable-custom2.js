$(document).ready(function() {
    // Setup - add a text input to each footer cell
    //$('#example thead tr').clone(true).appendTo( '#example thead' );
    //$('#example thead tr:eq(1) th').each( function (i) {
    //    var title = $(this).text();
    //    $(this).html( '<input type="text" placeholder="Search '+title+'" />' );
 
    //    $( 'input', this ).on( 'keyup change', function () {
    //        if ( table.column(i).search() !== this.value ) {
    //            table
    //                .column(i)
    //                .search( this.value )
    //                .draw();
    //        }
    //    } );
    //} );  

    $('#example thead tr').clone(true).appendTo('#example thead');
    $('#example thead tr:eq(1) th').each(function (i) {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="Search ' + title + '" />');

        $('input', this).on('keyup change', function () {
            if (table.column(i).search() !== this.value) {
                table
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
    });
 
    var table = $('#example').DataTable({
        //orderCellsTop: false,
        
        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [{
            extend: 'excel',
            text: 'Export to Excel'
        },
        ],
        "ordering": false
    });

    var table = $('#example1').DataTable({
        //orderCellsTop: false,

        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        //buttons: [{
        //    extend: 'excel',
        //    text: 'Export to Excel'
        //},
        //],
        "ordering": false
    });

 var table = $('#example').DataTable();
    table.columns('.select-filter').every(function () {
        var that = this;

        // Create the select list and search operation
        var select = $('<select />')
            .appendTo(
                this.footer()
            )
            .on('change', function () {
                that
                    .search($(this).val())
                    .draw();
            });

        // Get the search data for the first column and add to the select list
        this
            .cache('search')
            .sort()
            .unique()
            .each(function (d) {
                select.append($('<option value="' + d + '">' + d + '</option>'));
            });
    });

} );