// quicksort algorithm
function quicksort(arr, left, right) {
  if (left < right) {
    let pivot = partition(arr, left, right);
    quicksort(arr, left, pivot - 1);
    quicksort(arr, pivot + 1, right);
  }
}