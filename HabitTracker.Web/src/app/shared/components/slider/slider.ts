import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';


@Component({
  selector: 'app-slider',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './slider.html',
  styleUrls: ['./slider.css']
})
export class Slider {
  currentSlide = 0;

  slides = [
    {
      title: 'How to Build a New Habit',
      description: 'Make progress in your health, happiness, and your life.',
      button: 'Learn more',
      image: 'assets/images/slide-img.webp'
    },
    {
      title: 'Stay Consistent',
      description: 'Track your daily habits and build a better you.',
      button: 'Start Now',
      image: 'assets/images/slide-img.webp'
    },
    {
      title: 'Visualize Progress',
      description: 'See your streaks and celebrate your wins.',
      button: 'View Stats',
      image: 'assets/images/slide-img.webp'
    },
    {
      title: 'Join the Community',
      description: 'Share your journey and motivate others.',
      button: 'Connect',
      image: 'assets/images/slide-img.webp'
    }
  ];

  ngOnInit() {
    setInterval(() => {
      this.currentSlide = (this.currentSlide + 1) % this.slides.length;
    }, 4000);
  }
}
