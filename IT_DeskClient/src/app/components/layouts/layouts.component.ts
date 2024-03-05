import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';


@Component({
  selector: 'app-layouts',
  standalone: true,
  imports: [MenubarModule, RouterOutlet, InputTextModule, ButtonModule],
  templateUrl: './layouts.component.html',
  styleUrl: './layouts.component.css',
})
export class LayoutsComponent implements OnInit {
  itemsStart: MenuItem[] | undefined;

  ngOnInit() {
    this.itemsStart = [
      {
        label: 'Anasayfa',
        icon: 'pi pi-fw pi-home',
        routerLink: '/',
        style: { 'font-size': '20px', 'margin-left': '20px' },
      },
    ];
  }
}
