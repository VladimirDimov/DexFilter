import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  ColDef,
  GridOptions,
  GridReadyEvent,
  IServerSideDatasource,
} from 'ag-grid-community';
import 'ag-grid-enterprise';
import * as moment from 'moment';

@Component({
  selector: 'app-dates-example',
  templateUrl: './custom-orderby.component.html',
})
export class CustomOrderByComponent {
  public columnDefs: ColDef[] = [
    {
      field: 'firstName',
      headerName: 'First Name',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'lastName',
      headerName: 'Last Name',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'grade',
      headerName: 'Grade',
      filter: 'number',
      sortable: true,
    },
    {
      field: 'birthDate',
      headerName: 'Date Of Birth',
      filter: 'date',
      sortable: true,
      valueFormatter: (p) => this.dateFormatter(p.data.birthDate),
    },
    {
      field: 'address.city.name',
      headerName: 'City',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'address.street',
      headerName: 'Street',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'address.postalCode',
      headerName: 'Postal Code',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'subjects',
      headerName: 'Subjects',
      filter: 'text',
      sortable: true,
      minWidth: 400,
      valueFormatter: (p) => p.data.subjects.map((s: any) => s.name).join(', '),
    },
  ];

  public defaultColDef: ColDef = {
    flex: 1,
    minWidth: 100,
  };
  public rowModelType = 'serverSide';
  public rowData!: any[];
  categories: any[] = [];
  gridOptions: GridOptions = {
    columnDefs: this.columnDefs,
  };

  private customFilters = {};

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  onGridReady(params: GridReadyEvent) {
    var datasource = this.createServerSideDatasource();
    // register the datasource with the grid
    params.api!.setServerSideDatasource(datasource);
  }

  createServerSideDatasource(): IServerSideDatasource {
    const internalThis = this;
    return {
      getRows: function (params) {
        const requestOptions = {
          headers: {
            offset: new Date().getTimezoneOffset().toString(),
          },
        };

        let request = params.request;
        request.filterModel = {
          ...request.filterModel,
          ...internalThis.customFilters,
        };

        internalThis.http
          .post<any[]>(
            'http://localhost:5238/students/custom-orderby-example',
            params.request,
            requestOptions
          )
          .subscribe(
            (data: any) => {
              params.success({
                rowData: data.rowData,
                rowCount: data.rowCount,
              });
            },
            () => params.fail()
          );
      },
    };
  }

  resetFilters(): void {
    this.gridOptions.api?.setFilterModel(null);
  }

  dateFormatter(date: string) {
    if (!date) return '-';
    return moment(date).format('DD-MM-YYYY');
  }
}
