import { Component } from '@angular/core';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {
  DialogService,
  DynamicDialogModule,
  DynamicDialogRef,
} from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { CreateComponent } from '../create/create.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    BreadcrumbModule,
    TableModule,
    TagModule,
    InputTextModule,
    ButtonModule,
    DynamicDialogModule,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  providers: [DialogService, MessageService],
})
export default class HomeComponent{


  customers: any[] = [
    {
      id: 1000,
      name: 'James Butt',
      country: {
        name: 'Algeria',
        code: 'dz',
      },
      company: 'Benton, John B Jr',
      date: '2015-09-13',
      status: 'unqualified',
      verified: true,
      activity: 17,
      representative: {
        name: 'Ioni Bowcher',
        image: 'ionibowcher.png',
      },
      balance: 70663,
    },
  ];
  selectedCustomers!: any;
  ref: DynamicDialogRef | undefined;

  constructor(
    public dialogService: DialogService,
    public messageService: MessageService
  ) {}

  show() {
    this.ref = this.dialogService.open(CreateComponent, {
      header: 'Destek Oluştur',
      width: '50vw',
      contentStyle: { overflow: 'auto' },
      baseZIndex: 10000,
      maximizable: false,
      breakpoints: {
        '960px': '75vw',
        '640px': '90vw',
      },
    });

    this.ref.onClose.subscribe((data: any) => {
      if (data) {
        this.messageService.add({
          severity: 'info',
          summary: 'Product Selected',
          detail: data,
        });
      }
    });

    this.ref.onMaximize.subscribe((value) => {
      this.messageService.add({
        severity: 'info',
        summary: 'Maximized',
        detail: `maximized: ${value.maximized}`,
      });
    });
  }

  ngOnDestroy() {
    if (this.ref) {
      this.ref.close();
    }
  }

  getSeverity(status: string) {
    switch (status) {
      case 'unqualified':
        return 'danger';

      case 'qualified':
        return 'success';

      case 'new':
        return 'info';

      case 'negotiation':
        return 'warning';

      case 'renewal':
        return 'danger';
      default:
        return 'danger';
    }
  }
}