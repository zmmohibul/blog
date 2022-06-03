import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-plain-input',
  templateUrl: './plain-input.component.html',
  styleUrls: ['./plain-input.component.scss']
})
export class PlainInputComponent implements OnInit {
  @Input() label: string;
  @Input() control: FormControl;
  @Input() inputType: string;
  
  constructor() { }

  ngOnInit(): void {
  }

}
