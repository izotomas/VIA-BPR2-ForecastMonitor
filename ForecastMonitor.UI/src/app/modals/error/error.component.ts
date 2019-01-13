import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';

interface IErrorContent {
  operation: string;
  attempt?: number;
  body?: string;
}

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {
  attempt: number;
  operation: string;

  constructor(@Inject(MAT_DIALOG_DATA) private data: IErrorContent) {}

  ngOnInit() {
    if (this.data) {
      this.attempt = this.data.attempt;
      this.operation = this.data.operation;
    }
  }
}
