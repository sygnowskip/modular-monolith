import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {MenubarComponent} from './layout/menubar/menubar.component';
import {PlanExamModule} from './views/plan-exam/plan-exam.module';

@NgModule({
  declarations: [
    AppComponent,
    MenubarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PlanExamModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
