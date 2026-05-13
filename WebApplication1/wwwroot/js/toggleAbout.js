function toggleAbout() {
    var aboutDiv = document.getElementById('extraAbout');
    var learnBtn = document.getElementById('learnMoreBtn');
    if (aboutDiv.style.display === 'none') {
        aboutDiv.style.display = 'block';
        learnBtn.innerText = 'Show Less';
    } else {
        aboutDiv.style.display = 'none';
        learnBtn.innerText = 'Learn More';
    }
} 