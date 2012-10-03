class SayController < ApplicationController
  def hello
    @time = Time.now
  end

  def goodbye
  end

  def test
    @files = Dir.glob("*")
  end
end
