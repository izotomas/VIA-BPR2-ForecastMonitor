import { Component } from '@angular/core';
import { TranslationService } from '../../services/translation.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  brand: string;
  modelPerformance: string;

  constructor(private translationService: TranslationService) {
    this.translationService.fetchTranslations().subscribe(translations => {
      this.brand = translations.navigation.brand;
      this.modelPerformance = translations.navigation.modelPerformance;
    });
  }

  showTitle = () => location.href.split('/').includes('model');
}
