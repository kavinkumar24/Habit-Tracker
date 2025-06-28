import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Sidebar } from "../sidebar/sidebar";

@Component({
  selector: 'app-layout-component',
  imports: [RouterModule, Sidebar],
  templateUrl: './layout-component.html',
  styleUrl: './layout-component.css'
})
export class LayoutComponent {

}
