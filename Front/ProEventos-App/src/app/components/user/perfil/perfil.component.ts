import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  form!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validators();
  }


  validators(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'checkPassword')
    };

    this.form = this.fb.group({
      title: ['', [Validators.required]],
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      function: ['', [Validators.required]],
      description: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      password: ['', [Validators.required, Validators.minLength(7), Validators.maxLength(50)]],
      checkPassword: ['', [Validators.required, Validators.minLength(7), Validators.maxLength(50)]],

    }, formOptions);
  }

  //Conveniente para pega1r um formField apenas com a letra F
  get f(): any { return this.form.controls; }

  onSubmit(): void {

    //Vai parar aqui se o form estiver inválido
    if(this.form.invalid)
    {
      return;
    }
  }

  public resetForm(event: any): void {
    console.log(event);
    console.log(event.preventDefault());
    event.preventDefault();
    this.form.reset();
  }

}
